using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Interfaces;
using Infrastructure.Identity.Models;
using MediatR;

namespace Application.Commands.Authentication;

public class RefreshTokenLogin : IRequest<Result<TokenPair>>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenLoginCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<RefreshTokenLogin, Result<TokenPair>>
{

    public async Task<Result<TokenPair>> Handle(RefreshTokenLogin request, CancellationToken cancellationToken)
    {
        return await authenticationService.Authenticate(request.RefreshToken);
    }
}