using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chat.Queries;

 public record GetChatByIdQuery : IRequest<Result<ChatDto>>
 {
        public int Id { get; set; }
 }
 
internal class GetOfficeByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Result<ChatDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IMessageEncryptor _encryptor;

    public GetOfficeByIdQueryHandler (IMapper mapper, IUnitOfWork unitOfWork, IUser user, IMessageEncryptor encryptor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _user = user;
        _encryptor = encryptor;
    }

    public async Task<Result<ChatDto>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var chat = _unitOfWork.ChatRepository
                .FindByCondition(c => c.Id == request.Id)
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefault();
            if (chat == null || chat.Users.All(us => us.Id != _user.Id))
            {
                return await Result<ChatDto>.FailureAsync("Чат не найден");
            }

            var chatDto = new ChatDto()
            {
                Id = chat.Id,
                Name = chat.Name,
                IsActive = chat.IsActive,
                Owner = chat.Owner,
                Users = chat.Users,
                Messages = DecodeMessages(chat.Messages).ToList()
            };
            return await Result<ChatDto>.SuccessAsync(chatDto);
        }
        catch (Exception e)
        {
            return await Result<ChatDto>.FailureAsync($"Ошибка: {e.Message}");
        }
    }

    private IEnumerable<MessageDto> DecodeMessages(List<Message> messages)
    {
        var userKeysDictionary = new Dictionary<int, (byte[], byte[])>();
        foreach (var message in messages)
        {
            var key = Array.Empty<byte>();
            var iv = Array.Empty<byte>();
            if(userKeysDictionary.TryGetValue(message.IdUser, out var tokenData))
            {
                key = tokenData.Item1;
                iv = tokenData.Item2;
            }
            else
            {
                var keyData = _unitOfWork.UserRepository.FindByCondition(us => us.Id == message.IdUser).Include(us => us.Key).First().Key;
                userKeysDictionary.Add(message.IdUser, (keyData.Key, keyData.IV));
                key = keyData.Key;
                iv = keyData.IV;
            }
            var text = _encryptor.DecryptMessage(message.Text, key, iv);
            yield return new MessageDto(message, text);
        }
    }
}
