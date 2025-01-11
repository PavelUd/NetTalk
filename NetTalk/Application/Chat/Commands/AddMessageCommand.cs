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

public record AddMessageCommand : IRequest<Result<Message>>
{
    public int IdChat { get; set; }
    public string Text { get; set; }
}

internal class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result<Message>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public AddMessageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUser user)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _user = user;
    }

    public async Task<Result<Message>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = new Message()
            {
                IdChat = request.IdChat,
                Text = request.Text,
                UpdatedDate = DateTime.Now.ToUniversalTime(),
                CreatedDate = DateTime.Now.ToUniversalTime(),
                IdUser = _user.Id
            };
            var chat = _unitOfWork.ChatRepository.FindByCondition(us => us.Id == request.IdChat).Include(c => c.Messages).First();
            chat.Messages.Add(message);
            _unitOfWork.Commit();
            return await Result<Message>.SuccessAsync(message);
        }
        catch (Exception e)
        {
            return await Result<Message>.FailureAsync(e.Message);
        }
    }
}