using System.Diagnostics.CodeAnalysis;
using Application.Chat.Commands;
using Application.Queries.Chat;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Чаты")]
[ApiController]
[Route("api/chats")]
public class ChatController : Controller
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ChatController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Получить чат по id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatById(int id)
    {
        var query = new GetChatByIdQuery
        {
            Id = id
        };
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    /// <summary>
    /// Метод получения чатов
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetChats()
    {
        var query = new GetChatsQuery();
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    
    /// <summary>
    /// Метод создания групповых, персональных чатов
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}