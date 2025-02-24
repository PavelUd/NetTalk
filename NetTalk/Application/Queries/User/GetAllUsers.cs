using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Queries.QueryModels;
using Application.Users.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.User;

public class GetAllUsersQuery : IRequest<Result<List<UserQueryModel>>>
{
    
}

internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserQueryModel>>>
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;


    public GetAllUsersQueryHandler(IUserReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    
    public async Task<Result<List<UserQueryModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _readOnlyRepository.GetAllAsync();
        
        return await Result<List<UserQueryModel>>.SuccessAsync(users.ToList());
    }
}