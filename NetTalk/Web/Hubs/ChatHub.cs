using Application.Chat.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace NetTalk.Hubs;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(string idChat, string message, string url)
    {
        var query = new AddMessageCommand()
        {
            IdChat = int.Parse(idChat),
            Text = message
        };
       var result = await _mediator.Send(query);
       await Clients.All.SendAsync("ReceiveMessage",  JsonConvert.SerializeObject(result.Data), url);
    }
}