using Application.Commands.Chat;
using Application.Commands.Message;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace API.Hubs;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    
    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createMessage"></param>
    public async Task SendMessage(CreateMessage createMessage)
    {
        var result = await _mediator.Send(createMessage);
        await Clients.All.SendAsync("ReceiveMessage",  JsonConvert.SerializeObject(result.Data));
    }
}