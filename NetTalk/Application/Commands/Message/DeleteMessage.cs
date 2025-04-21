using Application.Common.Interfaces.Repositories.Commands;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Message;

public record DeleteMessage : IRequest<Result<bool>>
{
    public Guid IdChat { get; init; }
    public Guid IdMessage { get; init; }
}

public class DeleteMessageHandler : IRequestHandler<DeleteMessage, Result<bool>>
{
    private readonly IMessageRepository _repository;
    private readonly IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;
    
    public DeleteMessageHandler(IMessageRepository repository,IChatRepository chatRepository, IUser user, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _chatRepository = chatRepository;
        _user = user;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<bool>> Handle(DeleteMessage request, CancellationToken cancellationToken)
    {
        try
        {
           
            var message = _repository.FindByCondition(m => m.Id == request.IdMessage).FirstOrDefault();
            var result = ValidateMessage(request, message);
            if (!result.Succeeded)
            {
                return result;
            }
            UpdateChatLastMessage(message.IdChat, message);
            await _repository.DeleteAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return await Result<bool>.SuccessAsync(true);
        }
        catch (Exception ex)
        {
            return await Result<bool>.FailureAsync(ex.Message);
        }
    }

    private Result<bool> ValidateMessage(DeleteMessage request, Domain.Entities.Message message)
    {
        if (message == null)
        {
            return Result<bool>.Failure("Message not found");
        }

        if (message.IdUser != _user.Id)
        {
            return  Result<bool>.Failure("You cannot delete this message");
        }

        if (message.IdChat != request.IdChat)
        {
            return Result<bool>.Failure("You cannot delete this message");
        }

        return  Result<bool>.Success(true);
    }

    private void UpdateChatLastMessage(Guid chatId, Domain.Entities.Message message)
    {
        var chat = _chatRepository.FindByCondition(us => us.Id == chatId).FirstOrDefault();
        var lastMessage = _repository
            .FindByCondition(m => m.IdChat == message.IdChat)
            .OrderByDescending(m => m.CreatedDate)
            .FirstOrDefault();
        
        chat?.UpdateLastMessage(lastMessage);
    }
}