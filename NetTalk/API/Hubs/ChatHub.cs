using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IMessageEncryptor _encryptor;
}