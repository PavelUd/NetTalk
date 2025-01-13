using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Authentication.Queries;

public class GetJwtTokenQuery : IRequest<Result<string>>
{
    public string Login { get; set; }
    public string Password { get; set; }
}

internal class GetJwtTokenQueryHandler(IAuthenticationService service, IUnitOfWork unitOfWork)
    : IRequestHandler<GetJwtTokenQuery, Result<string>>
    {
    public async Task<Result<string>> Handle(GetJwtTokenQuery request, CancellationToken cancellationToken)
    {
        return await service.Authenticate(request.Login, request.Password);
    }
}
