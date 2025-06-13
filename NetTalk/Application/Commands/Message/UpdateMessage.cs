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
    public IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _repository;
    private readonly IUser _user;
    private readonly IMessageEncryptor _encryptor;

    public UpdateMessageHandler(IUnitOfWork unitOfWork, IChatRepository chatRepository, IUserRepository userRepository,
        IMessageRepository repository, IUser user, IMessageEncryptor encryptor)
    {
        _chatRepository = chatRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _repository = repository;
        _user = user;
        _encryptor = encryptor;
    }

    public async Task<Result<MessageDto>> Handle(UpdateMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var message = _repository.FindByCondition(x => x.Id == request.IdMessage).FirstOrDefault();
            
            var validationResult = ValidateMessage(request, message);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            var encryptText = EncryptMessage(request.Text);
            await UpdateMessage(message, encryptText, cancellationToken);
            
            return await Result<MessageDto>.SuccessAsync(new MessageDto(message, request.Text, _user));
        }
        catch (Exception e)
        {
            return await Result<MessageDto>.FailureAsync(e.Message);
        }
    }
    
    private Result<MessageDto> ValidateMessage(UpdateMessage request, Domain.Entities.Message message)
    {
        if (message == null) 
            return Result<MessageDto>.Failure("Message not found");

        if (message.IdUser != _user.Id)
            return Result<MessageDto>.Failure("You can't update this message");

        if (message.IdChat != request.IdChat)
            return Result<MessageDto>.Failure("You can't update this message");

        return Result<MessageDto>.Success(null);
    }

    private async Task UpdateChatLastMessage(Guid chatId, Domain.Entities.Message message, CancellationToken cancellationToken)
    {
        var lastMessage = await _repository.FindByCondition(m => m.IdChat == message.IdChat)
            .OrderByDescending(m => m.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);
        var chat = _chatRepository.FindByCondition(us => us.Id == chatId).FirstOrDefault();
        if (lastMessage == null || lastMessage.Id == message.Id)
        {
            chat?.UpdateLastMessage(message);
        }
    }
    
    private async Task UpdateMessage(Domain.Entities.Message message, byte[] newText, CancellationToken cancellationToken)
    {
        message.Text = newText;
        message.UpdatedDate = DateTime.Now.ToUniversalTime();
        await _repository.UpdateAsync(message);
        await UpdateChatLastMessage(message.IdChat, message, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
    }

    
    private byte[] EncryptMessage(string message)
    {
        var user = _userRepository.FindByCondition(us => _user.Id == us.Id).Include(us => us.Key).FirstOrDefault();
        var iv = user.Key.IV;
        var key = user.Key.Key;
        var encryptMessage = _encryptor.EncryptMessage(key, iv, message);
        return encryptMessage;
    }
}