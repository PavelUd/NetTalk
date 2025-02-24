using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chat.Commands;

public class CreateChatCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<User> Users { get; set; }
    
}

internal class  CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Result<Guid >>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;

    public CreateChatCommandHandler(IUnitOfWork unitOfWork, IUser user)
    {
        _unitOfWork = unitOfWork;
        _user = user;
    }

    public async Task<Result<Guid >> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var user in request.Users)
            {
                _unitOfWork.UserRepository.Attach(user);
            }
            var chat = new Domain.Entities.Chat(request.Name, request.Type,true, Guid.Empty, request.Users);
            await _unitOfWork.ChatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();
             return await Result<Guid>.SuccessAsync(chat.Id);
        }
        catch (Exception e)
        {
            return await Result<Guid >.FailureAsync($"Ошибка при созданни чата: {e.Message}");
        }
    }
    
    
}