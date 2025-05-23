using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chat.Commands;

public class UpdateMessageCommand : IRequest<Result<MessageDto>>
{
    public int IdMessage { get; set; }
    public string Text { get; set; }
}

internal class  UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Result<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;
    private readonly IMessageEncryptor _encryptor;

    public UpdateMessageCommandHandler(IUnitOfWork unitOfWork, IUser user, IMessageEncryptor encryptor)
    {
        _unitOfWork = unitOfWork;
        _user = user;
        _encryptor = encryptor;
    }

    public async Task<Result<MessageDto>> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var chat = _unitOfWork.ChatRepository.FindByCondition(ch => ch.Messages.Any(m => m.Id == request.IdMessage))
                .Include(ch => ch.Messages).FirstOrDefault();
            if (chat == null)
            {
                return await Result<MessageDto>.FailureAsync("Chat not found");
            }

            var message = chat.Messages.FirstOrDefault(m => m.Id == request.IdMessage);
            if (message == null)
            {
                return await Result<MessageDto>.FailureAsync("Message not found");
            }


            var encryptedMessage = await EncryptMessage(request.Text);
            if (!encryptedMessage.Succeeded)
            {
                return await Result<MessageDto>.FailureAsync(encryptedMessage.Errors);
            }

            message.Text = encryptedMessage.Data;
            message.UpdatedDate = DateTime.Now.ToUniversalTime();
            _unitOfWork.ChatRepository.UpdateAsync(chat);
            _unitOfWork.Commit();
            var dto = new MessageDto(message, request.Text, new User
            {
                AvatarUrl = _user.AvatarUrl,
                FullName = _user.Name,
                Id = _user.Id
            });
            return await Result<MessageDto>.SuccessAsync(dto);
        }

        catch (Exception ex)
        {
            return await Result<MessageDto>.FailureAsync(ex);
        }
    }
    
    private async Task<Result<byte[]>> EncryptMessage(string message)
    {
        var user = _unitOfWork.UserRepository.FindByCondition(us => _user.Id == us.Id).Include(us => us.Key).FirstOrDefault();
        var iv = user.Key.IV;
        var key = user.Key.Key;
        var encryptMessage = _encryptor.EncryptMessage(key, iv, message);
        return await Result<byte[]>.SuccessAsync(encryptMessage);
    }
}