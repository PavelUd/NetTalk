using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chat.Commands;

public class DeleteMessageCommand: IRequest<Result<bool>>
{
    public int IdMessage { get; set; }
}

internal class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;

    public DeleteMessageCommandHandler(IUnitOfWork unitOfWork, IUser user)
    {
       
        _unitOfWork = unitOfWork;
        _user = user;
    }
    public async Task<Result<bool>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var chat = _unitOfWork.ChatRepository.FindByCondition(ch => ch.Messages.Any(m => m.Id == request.IdMessage))
                .Include(ch => ch.Messages).FirstOrDefault();
            if (chat == null)
            {
                return await Result<bool>.FailureAsync("Chat not found");
            }
            var message = chat.Messages.FirstOrDefault(m => m.Id == request.IdMessage);
            if (message == null)
            {
                return await Result<bool>.FailureAsync("Message not found");
            }
            chat.Messages.Remove(message);
            _unitOfWork.ChatRepository.UpdateAsync(chat);
            _unitOfWork.Commit();
            return await Result<bool>.SuccessAsync(true);
        }

        catch (Exception ex)
        {
            return await Result<bool>.FailureAsync(ex);
        }
    }
}