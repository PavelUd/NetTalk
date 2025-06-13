using System.Net.Mime;
using Application.Commands.Chat;
using Application.Commands.Message;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    public async Task InvateChat(Guid idChat)
    {
        var groupName = idChat.ToString();
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createMessage"></param>
    public async Task SendMessage(Guid idChat, string message)
    {
        var command = new CreateMessage()
        {
            IdChat = idChat,
            Text = message
        };
        var result = await _mediator.Send(command);
        await Clients.All.SendAsync("ReceiveMessage",  JsonConvert.SerializeObject(result.Data));
    }
    
    public async Task DeleteMessage(Guid idChat, Guid idMessage)
    {
        var command = new DeleteMessage()
        {
            IdChat = idChat,
            IdMessage = idMessage
        };
        var result = await _mediator.Send(command);
        if (!result.Succeeded)
        {
            await Clients.All.SendAsync("ReceiveDeleteMessage", JsonConvert.SerializeObject(result.Data));
        }
        await Clients.All.SendAsync("ReceiveDeleteMessage",  JsonConvert.SerializeObject(idMessage));
    }
    
    public async Task UpdateMessage(Guid idChat,string text, Guid idMessage)
    {
        var command = new UpdateMessage()
        {
            IdChat = idChat,
            Text = text,
            IdMessage = idMessage
        };
        var result = await _mediator.Send(command);
        await Clients.All.SendAsync("ReceiveUpdateMessage",  JsonConvert.SerializeObject(result.Data));
    }
}