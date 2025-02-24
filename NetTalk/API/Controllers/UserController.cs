using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Пользователи")]
[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Возврат пользователелей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}