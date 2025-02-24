using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Queries.QueryModels;
using Application.Users.Dto;
using MediatR;

namespace Application.Queries.User;

internal record GetUsersByIdQuery : IRequest<Result<UserQueryModel>>
{
    public Guid Id { get; set; }
}

internal class GetUsersByIdQueryHandler : IRequestHandler<GetUsersByIdQuery, Result<UserQueryModel>>
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;

    public GetUsersByIdQueryHandler(IUserReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    
    public async Task<Result<UserQueryModel>> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _readOnlyRepository.GetByIdAsync(request.Id);
            return await Result<UserQueryModel>.SuccessAsync(result);
        }
        catch (Exception e)
        {
            return await Result<UserQueryModel>.FailureAsync(e.Message);
        }
    }
}