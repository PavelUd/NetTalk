using Application.Queries.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace API.Hubs;

[Authorize]
public class ChatListHub : Hub
{
    private readonly IMediator _mediator;
    
    public ChatListHub(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    public async Task GetChats()
    {
        var chatListQuery = new GetChatsQuery();
        var result = await _mediator.Send(chatListQuery);
        try
        {
            await Clients.Caller.SendAsync("ReceiveChatsList", result.Data);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отправке сообщения: " + ex.Message);
        }
    }
}