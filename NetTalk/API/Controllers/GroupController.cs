using Application.Commands.Chat.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Групповые чаты")]
[ApiController]
[Route("api/chats/groups")]
public class GroupController : Controller
{
    private readonly IMediator _mediator;
    
    public GroupController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Created($"/api/chats/{result.Data}", new { id = result.Data }) : BadRequest(result);
    }
    
}