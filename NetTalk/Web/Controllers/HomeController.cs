using System.Diagnostics;
using Application.Chat.Dto;
using Application.Queries.Chat;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetTalk.Models;
using GetChatsQuery = Application.Queries.Chat.GetChatsQuery;

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
        var chats = await GetChatsList();
       return View(chats);
    }
    
    [Route("chats/{id}")]
    public async Task<IActionResult> Chat(int id)
    {
        return Ok();
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

    private async Task<ChatListModel> GetChatsList()
    {
        var query = new GetChatsQuery();
        var usersQuery = new GetAllUsersQuery();
        var userResult = await _mediator.Send(usersQuery);
        var result = await _mediator.Send(query);
        var model = new ChatListModel()
        {
            Chats = result.Data,
            OtherUsers = userResult.Data
        };
        return model;
    }
}