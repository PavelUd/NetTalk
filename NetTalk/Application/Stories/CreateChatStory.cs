using Application.Chat.Commands;
using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Interfaces;
using Application.Queries.QueryModels;
using Application.Queries.User;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Stories;

public class CreateChatStory(IMediator mediator, IUser user, IMapper mapper) : IStory
{
    
    public async Task<Result<Guid>> Handle(string type, List<Guid> idUsers, string name = "")
    {
        try
        {
            var command = new CreateChatCommand()
            {
                Type = type,
                Name = name,
                Users = await GetUsers(idUsers),
            };
            return await mediator.Send(command);
        }

        catch (Exception e)
        {
            return await Result<Guid>.FailureAsync(e.Message);
        }
    }

    private async Task<List<User>> GetUsers(List<Guid> idUsers)
    {
        var users = new List<UserQueryModel>();
        foreach (var id in idUsers)
        {
            var user = await mediator.Send(new GetUsersByIdQuery()
            {
                Id = id
            });
            users.Add(user.Data);
        }
        return mapper.Map<List<User>>(users);
    }
    
}