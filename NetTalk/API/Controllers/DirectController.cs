using Application.Commands.Chat.Create;
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
}