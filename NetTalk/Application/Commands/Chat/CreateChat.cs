using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chat.Commands;

public class CreateChatCommand : IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<int> IdUsers { get; set; }
    


}

internal class  CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;

    public CreateChatCommandHandler(IUnitOfWork unitOfWork, IUser user)
    {
        _unitOfWork = unitOfWork;
        _user = user;
    }

    public async Task<Result<int>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _unitOfWork.UserRepository
                .FindByCondition(u => request.IdUsers.Contains(u.Id))
                .ToListAsync(cancellationToken: cancellationToken);
            var chat = new Domain.Entities.Chat(request.Name, request.Type,true, 1, users);
            await _unitOfWork.ChatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();
             return await Result<int>.SuccessAsync(chat.Id);
        }
        catch (Exception e)
        {
            return await Result<int>.FailureAsync($"Ошибка при созданни чата: {e.Message}");
        }
    }
    
    
}