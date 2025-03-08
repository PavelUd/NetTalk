using Application.Commands.Chat;
using Application.Commands.Chat.Create;
using Application.Commands.Message;
using Application.Common.Interfaces;
using Application.Queries.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Чаты")]
[ApiController]
[Route("api/chats")]
public class ChatController : Controller
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить чат по id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatById(Guid  id)
    {
        var result = await _mediator.Send(new GetChatByIdQuery { Id = id });
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    /// <summary>
    /// Метод получения чатов
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetChats()
    {
        var result = await _mediator.Send(new GetChatsQuery());
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    
    [Authorize]
    [HttpPost("{id}/messages")]
    public async Task<IActionResult> CreateMessage(Guid id, [FromBody] string message)
    {
        var command = new CreateMessage
        {
            IdChat = id,
            Text = message
        };
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [Authorize]
    [HttpDelete("{id}/messages{idMessage}")]
    public async Task<IActionResult> DeleteMessage(Guid id, Guid idMessage)
    {
        var command = new DeleteMessage()
        {
            IdChat = id,
            IdMessage = idMessage
        };
        var result = await _mediator.Send(command);
        return result.Succeeded ? NoContent() : BadRequest(result);
    }
    
    [Authorize]
    [HttpPatch("{id}/messages{idMessage}")]
    public async Task<IActionResult> UpdateMessage(Guid id, Guid idMessage, string text)
    {
        var command = new UpdateMessage()
        {
            IdChat = id,
            IdMessage = idMessage,
            Text = text
        };
        var result = await _mediator.Send(command);
        return result.Succeeded ? NoContent() : BadRequest(result);
    }
}