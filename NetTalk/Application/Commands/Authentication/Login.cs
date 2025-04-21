using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Identity.Models;
using MediatR;

namespace Application.Queries.Authentication;

public class GetJwtTokenQuery : IRequest<Result<TokenPair>>
{
    public string Login { get; set; }
    public string Password { get; set; }
}

internal class GetJwtTokenQueryHandler(IAuthenticationService service)
    : IRequestHandler<GetJwtTokenQuery, Result<TokenPair>>
    {
    public async Task<Result<TokenPair>> Handle(GetJwtTokenQuery request, CancellationToken cancellationToken)
    {
        return await service.Authenticate(request.Login, request.Password);
    }
}
