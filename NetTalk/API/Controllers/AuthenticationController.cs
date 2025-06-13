using Application.Authentication.Command;
using Application.Commands.Authentication;
using Application.Common.Result;
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

        if (!token.Succeeded) 
            return Unauthorized(new { message = "Неверный логин или пароль" });
        
        var refreshToken = token.Data.RefreshToken;
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        var result = new Result<string>()
        {
            Data = token.Data.AccessToken,
        };
        return Ok(result);

    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Authenticate()
    {
        var oldRefreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(oldRefreshToken))
            return Unauthorized();
        var query = new RefreshTokenLogin()
        {
            RefreshToken = oldRefreshToken
        };
        var token = await _mediator.Send(query);

        if (!token.Succeeded)
            return Unauthorized(new { message = "Неверный логин или пароль" });
        
        var refreshToken = token.Data.RefreshToken;
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        var result = new Result<string>()
        {
            Data = token.Data.AccessToken,
        };
        return Ok(result);

    }

    
    
    /// <summary>
    /// Метод подтверждения регистрации 
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("verify")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmCommand query)
    {
        var token = await _mediator.Send(query);

        if (!token.Succeeded) 
            return BadRequest(new { message = token.Errors });
        
        var refreshToken = token.Data.RefreshToken;
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        var result = new Result<string>()
        {
            Data = token.Data.AccessToken,
        };
        return Ok(result);
    }
        
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
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> SendRequestPasswordReset([FromBody] RequestPasswordReset query)
    {
        var token = await _mediator.Send(query);

        if (token.Succeeded)
        {
            return Ok(token);
        }

        return Unauthorized(new { message = token.Errors });
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> SendRequestPasswordReset([FromBody] ResetPassword query)
    {
        var token = await _mediator.Send(query);

        if (token.Succeeded)
        {
            return Ok(token);
        }

        return Unauthorized(new { message = token.Errors });
    }
}