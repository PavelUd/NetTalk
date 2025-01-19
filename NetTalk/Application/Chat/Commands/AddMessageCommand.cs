using Application.Chat.Dto;
using Application.Chat.Queries;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Chat.Commands;

public record AddMessageCommand : IRequest<Result<MessageDto>>
{
    public int IdChat { get; set; }
    public string Text { get; set; }
}

internal class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IMessageEncryptor _encryptor;
    private readonly IMediator _mediator;

    public AddMessageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUser user, IMessageEncryptor encryptor, IMediator mediator)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _user = user;
        _encryptor = encryptor;
        _mediator = mediator;
    }

    public async Task<Result<MessageDto>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        
        try
        {
            var encryptedMessage = await EncryptMessage(request.Text);
            if (!encryptedMessage.Succeeded)
            {
                return await Result<MessageDto>.FailureAsync(encryptedMessage.Errors);
            }
            var chat = _unitOfWork.ChatRepository
                .FindByCondition(us => us.Id == request.IdChat).Include(c => c.Messages)
                .FirstOrDefault();
            var message = new Message()
            {
                IdChat = request.IdChat,
                Text = encryptedMessage.Data,
                UpdatedDate = DateTime.Now.ToUniversalTime(),
                CreatedDate = DateTime.Now.ToUniversalTime(),
                IdUser = _user.Id
            };
            chat.Messages.Add(message);
            _unitOfWork.Commit();
            var dto = new MessageDto(message, request.Text, new User
            {
                AvatarUrl = _user.AvatarUrl,
                FullName = _user.Name,
                Id = _user.Id
            });
            return await Result<MessageDto>.SuccessAsync(dto);
        }
        catch (Exception e)
        {
            return await Result<MessageDto>.FailureAsync(e.Message);
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