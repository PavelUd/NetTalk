using Application.Chat.Dto;
using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Queries.QueryModels;
using Application.Queries.User;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace Application.Queries.Chat;

public record GetChatsQuery : IRequest<Result<List<ChatQueryModel>>>
{
}


internal class GetChatsQueryQueryHandler : IRequestHandler<GetChatsQuery, Result<List<ChatQueryModel>>>
{
    public IUserRepository _keyRepository;
    private readonly IMessageEncryptor _messageEncryptor;
    private readonly IUser _user;
    private readonly IChatReadOnlyRepository _chatRepository;
    private readonly IUserReadOnlyRepository _userRepository;

    public GetChatsQueryQueryHandler(IUser user, IChatReadOnlyRepository readOnlyRepository,IUserRepository userRepository, IUserReadOnlyRepository userReadOnlyRepository, IMessageEncryptor messageEncryptor)
    {
        _keyRepository = userRepository;
        _user = user;
        _userRepository = userReadOnlyRepository;
        _messageEncryptor = messageEncryptor;
        _chatRepository = readOnlyRepository;
    }

    public async Task<Result<List<ChatQueryModel>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var chats = await _chatRepository.GetAllAsync();
            var chatList = chats.ToList();
            foreach (var chat in chatList)
            {
                if (chat.LastMessage != null)
                {
                    chat.LastMessage.Text = DecodeMessage(chat.LastMessage);
                }
                if (chat.Type != "Personal")
                    continue;

                var otherUserId = chat.Participants.First(id => id != _user.Id);
                var otherUser = await _userRepository.GetByIdAsync(otherUserId);
                chat.Name = otherUser.Email;

                chat.Url = otherUser.Avatar;
            }
            return await Result<List<ChatQueryModel>>.SuccessAsync(chatList);
        }
        catch (Exception e)
        {
           return await Result<List<ChatQueryModel>>.FailureAsync(e.Message);
        }
    }
    
    private string DecodeMessage(MessageQueryModel messageQueryModel)
    {
            var user = _keyRepository.FindByCondition(u => u.Id == messageQueryModel.IdUser).Include(user => user.Key).First();
            var text = _messageEncryptor.DecryptMessage(messageQueryModel.EncryptText, user.Key.Key, user.Key.IV);
            return text;
    }
}