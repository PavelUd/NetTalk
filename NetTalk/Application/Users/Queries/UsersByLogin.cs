using Application.Common.Result;
using Application.Interfaces.Repositories;
using Application.Users.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public class GetUsersByLoginQuery : IRequest<Result<List<UserDto>>>
{
    public string Login { get; set; }
}

internal class GetUsersByLoginQueryHandler : IRequestHandler<GetUsersByLoginQuery, Result<List<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetUsersByLoginQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetUsersByLoginQuery request, CancellationToken cancellationToken)
    {
        var users = _unitOfWork.UserRepository.
            FindByCondition(us => us.Login == request.Login)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider);
        return await Result<List<UserDto>>.SuccessAsync(await users.ToListAsync(cancellationToken: cancellationToken));
    }
}