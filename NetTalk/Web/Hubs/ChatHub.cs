using Application.Chat.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace NetTalk.Hubs;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IMessageEncryptor _encryptor;

    public ChatHub(IMediator mediator, IMessageEncryptor encryptor)
    {
        _mediator = mediator;
        _encryptor = encryptor;
    }

    public async Task JoinPrivateChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }
    
    public async Task SendMessage(string idChat, string message, string url)
    {
        var query = new AddMessageCommand()
        {
            IdChat = int.Parse(idChat),
            Text = message
        };
       var result = await _mediator.Send(query);
       await Clients.Group(idChat).SendAsync("ReceiveMessage",  JsonConvert.SerializeObject(result.Data), url);
    }
}