using Application.Authentication.Command;
using Application.Queries.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Аутентификация")]
[ApiController]
[Route("api/auth")]
public class AuthenticationController : Controller
{
    
    private readonly IMediator _mediator;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Вход Пользователя
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] GetJwtTokenQuery query)
    {
         
        var token = await _mediator.Send(query);

        if (token.Succeeded)
        {
            return Ok(token);
        }

        return Unauthorized(new { message = "Неверный логин или пароль" });
    }
    
    /// <summary>
    /// Метод регистрации
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand query)
    {
        var token = await _mediator.Send(query);

        if (token.Succeeded)
        {
            return Ok(token);
        }

        return Unauthorized(new { message = token.Errors });
    }
}