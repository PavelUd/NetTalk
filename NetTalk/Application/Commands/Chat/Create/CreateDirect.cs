using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Chat.Create;

public record CreateDirectCommand : IRequest<Result<Guid>>
{
    public Guid IdOtherUser { get; set; }
}

internal class  CreateDirectCommandHandler : IRequestHandler<CreateDirectCommand, Result<Guid >>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;

    public CreateDirectCommandHandler(IUnitOfWork unitOfWork, IUser user, IUserRepository userRepository, IChatRepository chatRepository)
    {
        _unitOfWork = unitOfWork;
        _user = user;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    public async Task<Result<Guid>> Handle(CreateDirectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = _userRepository
                .FindByCondition(us => us.Id == request.IdOtherUser || us.Id == _user.Id)
                .ToList();

            if (users.Count != 2)
            {
                return await Result<Guid>.FailureAsync("Пользователи не найдены");
            }
            
            var personalChatExists = _chatRepository.FindByCondition(ch =>
                ch.Type == nameof(ChatType.Personal) &&
                ch.Users.Any(u => u.Id == _user.Id) &&
                ch.Users.Any(u => u.Id == request.IdOtherUser)
            ).Any();

            if (personalChatExists)
            {
                return await Result<Guid>.FailureAsync("Персональный чат уже существует");
            }
            
            var chat = new Domain.Entities.Chat(
                name: "",
                type: nameof(ChatType.Personal),
                isActive: true,
                owner: _user.Id,
                users: users
            );

            await _chatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            return await Result<Guid>.SuccessAsync(chat.Id);
        }
        catch (Exception e)
        {
            return await Result<Guid>.FailureAsync($"Ошибка при создании персонального чата: {e.Message}");
        }
    }
    
    
}