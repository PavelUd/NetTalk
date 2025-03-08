using Application.Chat.Dto;
using Application.Common.Interfaces.Repositories.Commands;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Message;

public record CreateMessage : IRequest<Result<MessageDto>>
{
    public Guid  IdChat { get; set; }
    public string Text { get; set; }
}

internal class AddMessageHandler : IRequestHandler<CreateMessage, Result<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _repository;
    private readonly IUser _user;
    private readonly IMessageEncryptor _encryptor;

    public AddMessageHandler(IMapper mapper, IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IChatRepository chatRepository, 
        IMessageRepository repository, 
        IUser user, 
        IMessageEncryptor encryptor)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _repository = repository;
        _user = user;
        _encryptor = encryptor;
    }

    public async Task<Result<MessageDto>> Handle(CreateMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var encryptedMessage = EncryptMessage(request.Text);
            var chat = _chatRepository.FindByCondition(us => us.Id == request.IdChat).FirstOrDefault();

            if (chat == null)
            {
                return await Result<MessageDto>.FailureAsync("Chat not found");
            }
            var message = new Domain.Entities.Message(request.IdChat, _user.Id, encryptedMessage);
            
            await _repository.AddAsync(message);
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