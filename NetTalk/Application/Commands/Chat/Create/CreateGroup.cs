using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Chat.Create;

public record CreateGroupCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public HashSet<Guid> Users { get; set; }
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<Guid >>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;

    public CreateGroupCommandHandler(IUnitOfWork unitOfWork, IUser user, IUserRepository userRepository, IChatRepository chatRepository)
    {
        _unitOfWork = unitOfWork;
        _user = user;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    public async Task<Result<Guid>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
 
            var users = _userRepository
                .FindByCondition(u => request.Users.Contains(u.Id))
                .ToList();


            if (users.Count != request.Users.Count)
            {
                return await Result<Guid>.FailureAsync("Не все указанные пользователи найдены");
            }
            
            var userOwner = _userRepository
                .FindByCondition(u => u.Id == _user.Id)
                .FirstOrDefault();

            if (userOwner == null)
            {
                return await Result<Guid>.FailureAsync("Чат не может быть создан, так как инициатор чата неизвестен или был удалён.");
            }
            
            if (users.All(u => u.Id != userOwner.Id))
            {
                users.Add(userOwner);
            }
            
            var chat = new Domain.Entities.Chat(request.Name, nameof(ChatType.Group), true, _user.Id, users);
            await _chatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            return await Result<Guid>.SuccessAsync(chat.Id);
        }
        catch (Exception e)
        {
            return await Result<Guid>.FailureAsync($"Ошибка при создании группового чата: {e.Message}");
        }
    }
    
    
}