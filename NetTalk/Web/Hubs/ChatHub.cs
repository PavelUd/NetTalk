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
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat{chatId}");
    }
    public async Task LeavePrivateChat(string chatId)
    {
        if (chatId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat{chatId}");
        }
    }

    public async Task InitPrivateChat(string message, List<int> idUsers)
    {
        var userId = Context.User!.Claims.FirstOrDefault(cl => cl.Type == "Id")?.Value;
        var otherUserId = idUsers.First(us => us.ToString() != userId);
        var command = new CreateChatCommand()
        {
            IdUsers = idUsers,
            Name = "",
            Type = "personal"
        };
        var result = await _mediator.Send(command);
        if (result.Succeeded)
        {
            await Clients.Group(userId).SendAsync("InitChat",  JsonConvert.SerializeObject(result.Data));
            await Clients.Group(otherUserId.ToString()).SendAsync("ReceiveInitChat",  JsonConvert.SerializeObject(result.Data));
            await JoinPrivateChat(result.Data.ToString());
            await SendMessage(result.Data.ToString(), message);
        }
    }
    
    public async Task SendMessage(string idChat, string message)
    {
        var query = new AddMessageCommand()
        {
            IdChat = int.Parse(idChat),
            Text = message
        };
       var result = await _mediator.Send(query);
       await Clients.Group($"Chat{idChat}").SendAsync("ReceiveMessage",  JsonConvert.SerializeObject(result.Data));
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User!.Claims.FirstOrDefault(cl => cl.Type == "Id")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
           await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId =  Context.User!.Claims.FirstOrDefault(cl => cl.Type == "Id")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}