using Application.Chat.Commands;
using Application.Queries.Chat;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTalk.Models;

namespace NetTalk.Controllers;

public class ChatController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ChatController(ILogger<HomeController> logger, IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("api/chats/{id}")] 
    public async Task<IActionResult> GetChatById(int id)
    {
        var query = new GetChatByIdQuery
        {
            Id = id
        };
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("api/chats")] 
    public async Task<IActionResult> GetChats()
    {
        var query = new GetChatsQuery();
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    
    [HttpPost("api/chats")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [HttpGet("api/chats/search")]
    public async Task<IActionResult> SearchChat([FromQuery] GetUsersByLoginQuery query)
    {
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
}