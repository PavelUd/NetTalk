using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Chat.Create;

public class CreateChatCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<Guid> Users { get; set; }
    
}

internal class  CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Result<Guid >>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;

    public CreateChatCommandHandler(IUnitOfWork unitOfWork, IUser user, IUserRepository userRepository, IChatRepository chatRepository)
    {
        _unitOfWork = unitOfWork;
        _user = user;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    public async Task<Result<Guid >> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users =  _userRepository.FindByCondition(us => request.Users.Contains(us.Id)).ToList();
            var chat = new Domain.Entities.Chat(request.Name, request.Type,true,_user.Id,users);
            await _chatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();
             return await Result<Guid>.SuccessAsync(chat.Id);
        }
        catch (Exception e)
        {
            return await Result<Guid >.FailureAsync($"Ошибка при созданни чата: {e.Message}");
        }
    }
    
    
}