using System.Diagnostics;
using Application.Chat.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTalk.Models;

namespace NetTalk.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var query = new GetChatsQuery()
        {
            Id = 1
        };
       var result = await _mediator.Send(query);
       return View(result.Data);
    }
    
    [Route("chats/{id}")]
    public async Task<IActionResult> Chat(int id)
    {
        var query = new GetChatByIdQuery()
        {
            Id = 1
        };
        var result = await _mediator.Send(query);
        return View(result.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}