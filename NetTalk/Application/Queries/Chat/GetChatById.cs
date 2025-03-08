using Application.Chat.Dto;
using Application.Commands.Chat.Dto;
using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Queries.QueryModels;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Chat;

 public record GetChatByIdQuery : IRequest<Result<ChatDto>>
 {
        public Guid Id { get; init; }
 }
 
internal class GetOfficeByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Result<ChatDto>>
{
    private readonly IMessageEncryptor _messageEncryptor;
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IChatReadOnlyRepository _chatRepository;
    private readonly IMessageReadOnlyRepository _messageRepository;
    private readonly IUserReadOnlyRepository _userRepository;

    public GetOfficeByIdQueryHandler(IMessageEncryptor messageEncryptor,IUserRepository writeUserRepository, IMapper mapper, IChatReadOnlyRepository repository, IMessageReadOnlyRepository messageRepository, IUserReadOnlyRepository userRepository)
    {
        _messageEncryptor = messageEncryptor;
        _repository = writeUserRepository;
        _mapper = mapper;
        _chatRepository = repository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<ChatDto>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var chat = await _chatRepository.GetByIdAsync(request.Id);
            var chatMessages = await _messageRepository.GetChatMessages(chat.Id);
            var members = await GetChatMembers(chat.Participants);
            var chatDto = _mapper.Map<ChatDto>(chat);
            chatDto.Users = members;
            chatDto.Messages = chatMessages.Select(DecodeMessage).ToList();
            
            return await Result<ChatDto>.SuccessAsync(chatDto);
        }

        catch (Exception ex)
        {
            return await Result<ChatDto>.FailureAsync(ex.Message);
        }
    }

    private async Task<List<UserQueryModel>> GetChatMembers(List<Guid> userIds)
    {
        var list = new List<UserQueryModel>();
        foreach (var id in  userIds)
        {
            var user = await _userRepository.GetByIdAsync(id);
            list.Add(user);
        }
        return list;
    }

    private MessageQueryModel DecodeMessage(MessageQueryModel messageQueryModel)
    {
        var user = _repository.FindByCondition(u => u.Id == messageQueryModel.IdUser).Include(user => user.Key).First();
        var text = _messageEncryptor.DecryptMessage(messageQueryModel.EncryptText, user.Key.Key, user.Key.IV);
        messageQueryModel.Text = text;
        return messageQueryModel;
    }
    
}
