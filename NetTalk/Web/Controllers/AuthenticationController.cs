using Application.Authentication.Command;
using Application.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTalk.Models;

namespace NetTalk.Controllers;

public class AuthenticationController : Controller
{
    
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationController(ILogger<HomeController> logger, IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [Route("login")]
    public async Task<IActionResult> SingIn()
    {
        return View();
    }
    [Route("register")]
    public async Task<IActionResult> Register()
    {
        return View();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest(new { message = "Логин и пароль обязательны" });
        }

        var query = new RegisterCommand()
        {
            Login = model.Login,
            Password = model.Password,
            FullName = model.FullName
        };
        var token = await _mediator.Send(query);

        if (token.Succeeded)
        {
            _httpContextAccessor.HttpContext.Session.SetString("AuthToken", token.Data);
            return Ok();
        }

        return Unauthorized(new { message = token.Errors });
    }

    [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] UserLoginModel model)
{
    if (model == null || string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
    {
        return BadRequest(new { message = "Логин и пароль обязательны" });
    }

    var query = new GetJwtTokenQuery()
    {
        Login = model.Login,
        Password = model.Password
    };
    var token = await _mediator.Send(query);

    if (token.Succeeded)
    {
        _httpContextAccessor.HttpContext.Session.SetString("AuthToken", token.Data);
        return Ok();
    }

    return Unauthorized(new { message = "Неверный логин или пароль" });
}
    
}