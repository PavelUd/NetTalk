using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Users.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.User;

public class GetAllUsersQuery : IRequest<Result<List<UserDto>>>
{
    
}

internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;


    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUser user)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _user = user;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _unitOfWork.UserRepository
            .FindByCondition(us => !us.Chats.Any(ch => ch.Users.Any(u => u.Id == _user.Id)))
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider);
        return await Result<List<UserDto>>.SuccessAsync(await users.ToListAsync(cancellationToken: cancellationToken));
    }
}