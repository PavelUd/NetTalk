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
                .Include(c => c.Users).ThenInclude(u => u.Key)
                .Include(c => c.Messages)
                .FirstOrDefault();
            if (chat == null || chat.Users.All(us => us.Id != _user.Id))
            {
                return await Result<ChatDto>.FailureAsync("Чат не найден");
            }

            var chatDto = new ChatDto()
            {
                Id = chat.Id,
                Name = chat.Users.First(us => us.Id != _user.Id).FullName,
                Url = chat.Users.First(us => us.Id != _user.Id).AvatarUrl,
                IsActive = chat.IsActive,
                Owner = chat.Owner,
                Messages = DecodeMessages(chat.Messages, chat.Users).OrderBy(c => c.CreatedDate).ToList()
            };
            return await Result<ChatDto>.SuccessAsync(chatDto);
        }
        catch (Exception e)
        {
            return await Result<ChatDto>.FailureAsync($"Ошибка: {e.Message}");
        }
    }

    private IEnumerable<MessageDto> DecodeMessages(List<Message> messages, List<User> users)
    {
        var userKeysDictionary = new Dictionary<int, User>();
        foreach (var message in messages)
        {
            var key = Array.Empty<byte>();
            var iv = Array.Empty<byte>();
            User user;
            if(userKeysDictionary.TryGetValue(message.IdUser, out var data))
            {
                user = data;
                key = data.Key.Key;
                iv = data.Key.IV;
            }
            else
            {
                var dataAdd = users.First(us => us.Id == message.IdUser);
                userKeysDictionary.Add(message.IdUser, (dataAdd));
                user = dataAdd;
                key = dataAdd.Key.Key;
                iv = dataAdd.Key.IV;
            }
            var text = _encryptor.DecryptMessage(message.Text, key, iv);
            yield return new MessageDto(message, text, user);
        }
    }
}
