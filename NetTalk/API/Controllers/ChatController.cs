using Application.Chat.Commands;
using Application.Common.Interfaces;
using Application.Queries.Chat;
using Application.Stories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Tags("Чаты")]
[ApiController]
[Route("api/chats")]
public class ChatController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStoryResolver _resolver;

    public ChatController(IMediator mediator, IStoryResolver resolver)
    {
        _mediator = mediator;
        _resolver = resolver;
    }

    /// <summary>
    /// Получить чат по id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatById(Guid  id)
    {
        var result = await _mediator.Send(new GetChatByIdQuery { Id = id });
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    /// <summary>
    /// Метод получения чатов
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetChats()
    {
        var result = await _mediator.Send(new GetChatsQuery());
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    
    /// <summary>
    /// Метод создания групповых, персональных чатов
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] List<Guid> idUsers)
    {
        var createChatStory = _resolver.Resolve<CreateChatStory>();
        var result = await createChatStory.Handle("group", idUsers, "chat");
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}