using Application.Chat.Dto;
using Application.Common.Interfaces.Repositories.Commands;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Message;

public record UpdateMessage : IRequest<Result<MessageDto>>
{
    public Guid IdMessage { get; init; }
    public string Text { get; init; }
    public Guid IdChat { get; init; }
}

public class UpdateMessageHandler : IRequestHandler<UpdateMessage, Result<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _repository;
    private readonly IUser _user;
    private readonly IMessageEncryptor _encryptor;

    public UpdateMessageHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IChatRepository chatRepository, IMessageRepository repository, IUser user, IMessageEncryptor encryptor)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _repository = repository;
        _user = user;
        _encryptor = encryptor;
    }

    public async Task<Result<MessageDto>> Handle(UpdateMessage request, CancellationToken cancellationToken)
    {
        try
        {


            var message = _repository.FindByCondition(x => x.Id == request.IdMessage).FirstOrDefault();
            if (message == null)
            {
                return await Result<MessageDto>.FailureAsync("Message not found");
            }

            if (message.IdUser != _user.Id)
            {
                return await Result<MessageDto>.FailureAsync("You can't update this message");
            }

            if (message.IdChat != request.IdChat)
            {
                return await Result<MessageDto>.FailureAsync("You can't update this message");
            }

            var encryptText = EncryptMessage(request.Text);
            message.Text = encryptText;
            await _repository.UpdateAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return await Result<MessageDto>.SuccessAsync(new MessageDto(message, request.Text, _user));
        }
        catch (Exception e)
        {
           return await Result<MessageDto>.FailureAsync(e.Message);
        }
    }
    
    private byte[] EncryptMessage(string message)
    {
        var user = _userRepository.FindByCondition(us => _user.Id == us.Id).Include(us => us.Key).FirstOrDefault();
        var iv = user.Key.IV;
        var key = user.Key.Key;
        var encryptMessage = _encryptor.EncryptMessage(key, iv, message);
        return  encryptMessage;
    }
}