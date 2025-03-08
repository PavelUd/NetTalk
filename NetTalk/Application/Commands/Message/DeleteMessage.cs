using Application.Chat.Dto;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;
    
    public DeleteMessageHandler(IMessageRepository repository, IUser user, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _user = user;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<bool>> Handle(DeleteMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var message = _repository.FindByCondition(m => m.Id == request.IdMessage).FirstOrDefault();
            if (message == null)
            {
                return await Result<bool>.FailureAsync("Message not found");
            }

            if (message.IdUser != _user.Id)
            {
                return await Result<bool>.FailureAsync("You cannot delete this message");
            }

            if (message.IdChat != request.IdChat)
            {
                return await Result<bool>.FailureAsync("You cannot delete this message");
            }
            
            await _repository.DeleteAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return await Result<bool>.SuccessAsync(true);
        }
        catch (Exception ex)
        {
            return await Result<bool>.FailureAsync(ex.Message);
        }
    }
}