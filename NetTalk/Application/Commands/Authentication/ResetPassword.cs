using Application.Common.Interfaces;
using Application.Common.Result;
using MediatR;

namespace Application.Commands.Authentication;

public record ResetPassword : IRequest<Result<string>>
{
    public string Token { get; init; }
    public string NewPassword { get; init; }
}

public class  ResetPasswordCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<ResetPassword, Result<string>>
{
    public async Task<Result<string>> Handle( ResetPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await authenticationService.ResetPasswordAsync(request.Token, request.NewPassword);
            return result;
        }
        catch (Exception ex)
        {
            return await Result<string>.SuccessAsync(ex.Message);
        }
    }
}