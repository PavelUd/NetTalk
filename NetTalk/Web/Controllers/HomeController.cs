using System.Diagnostics;
using Application.Chat.Dto;
using Application.Chat.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTalk.Models;
using GetChatsQuery = Application.Chat.Queries.GetChatsQuery;

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
        var chats = await GetUserChats();
       return View(chats);
    }
    
    [Route("chats/{id}")]
    public async Task<IActionResult> Chat(int id)
    {
        var query = new GetChatByIdQuery()
        {
            Id = id
        };
        var result = await _mediator.Send(query);
        if (!result.Succeeded)
        {
            return Redirect("/");
        }
        var chatViewModel = new ChatViewModel()
        {
            Chat = result.Data,
            AllChats = await GetUserChats()
        };
        return View(chatViewModel);
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

    private async Task<List<ChatSummary>> GetUserChats()
    {
        var query = new GetChatsQuery();
        var result = await _mediator.Send(query);
        return  result.Data;
    }
}