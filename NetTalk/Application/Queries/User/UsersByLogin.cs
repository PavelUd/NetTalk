using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Users.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Queries.User;

public class GetUsersByLoginQuery : IRequest<Result<List<UserDto>>>
{
    public string Login { get; set; }
}

internal class GetUsersByLoginQueryHandler : IRequestHandler<GetUsersByLoginQuery, Result<List<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;


    public GetUsersByLoginQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUser user, IChatRepository chatRepository, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        this._user = user;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetUsersByLoginQuery request, CancellationToken cancellationToken)
    {
        var users =  _userRepository
                .FindByCondition(us =>us.Id != _user.Id && us.FullName.ToLower().StartsWith(request.Login.ToLower()))
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToList();
        foreach (var user in users)
        {
            var ids = new Guid [] { user.Id, _user.Id };
            var chat =  _chatRepository
                .FindByCondition(c => ids.All(id => c.Users.Any(us => us.Id == id))).FirstOrDefault();
        }
        return await Result<List<UserDto>>.SuccessAsync( users);
    }
}