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
using MongoDB.Driver.Linq;

namespace Application.Queries.User;

public class GetAllUsersQuery : IRequest<Result<List<UserQueryModel>>>
{
    public string? Login  { get; set; }
}

internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserQueryModel>>>
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUser _user;

    public GetAllUsersQueryHandler(IUserReadOnlyRepository readOnlyRepository, IUser user)
    {
        _user = user;
        _readOnlyRepository = readOnlyRepository;
    }
    
    public async Task<Result<List<UserQueryModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _readOnlyRepository.GetAllAsync();
        
        return await Result<List<UserQueryModel>>.SuccessAsync(users.Where(us => us.Id != _user.Id && (request.Login == null || us.Email.StartsWith(request.Login, StringComparison.OrdinalIgnoreCase))).ToList());
    }
}