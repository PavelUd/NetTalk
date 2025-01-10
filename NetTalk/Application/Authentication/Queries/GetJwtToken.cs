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
        try
        {
            var user = unitOfWork.UserRepository
                .FindByCondition(u => u.Login == request.Login && u.Password == request.Password)
                .FirstOrDefault();
            if (user == null)
            {
                return await Result<string>.FailureAsync("Пользователь не найден");
            }
            var token = await service.Authenticate(user);
            return await Result<string>.SuccessAsync(token);
        }
        catch (Exception e)
        {
            return await Result<string>.FailureAsync(e.Message);
        }
    }
}
