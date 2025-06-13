using Application.Commands.Chat.Create;
using Application.Queries.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Персональные чаты")]
[ApiController]
[Route("api/chats/direct")]
public class DirectController : Controller
{
    private readonly IMediator _mediator;
    
    public DirectController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateDirect([FromBody] CreateDirectCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ?  Created($"/api/chats/{result.Data}", new { id = result.Data }) : BadRequest(result);
    }
 
    [Authorize]
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetDirectByIdUser(Guid id)
    {
        var command = new GetDirectIdByUserId()
        {
            IdOtherUser = id
        };
        var idChatResult = await _mediator.Send(command);
        if (!idChatResult.Succeeded)
        {
            return BadRequest(idChatResult);
        }

        if (idChatResult.Data == null)
        {
            return Ok(idChatResult);
        }
        var chatResult =  await _mediator.Send(new GetChatByIdQuery()
        {
            Id = idChatResult.Data.Value
        });
        if (!chatResult.Succeeded)
        {
            return BadRequest(chatResult);
        }
        return Ok(chatResult);
    }
}