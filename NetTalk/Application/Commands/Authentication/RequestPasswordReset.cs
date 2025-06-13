using Application.Common.Interfaces;
using Application.Common.Result;
using MediatR;

namespace Application.Commands.Authentication;

public record RequestPasswordReset : IRequest<Result<string>>
{
    public string Email { get; set; }
}

public class RequestPasswordResetCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<RequestPasswordReset, Result<string>>
{
    public async Task<Result<string>> Handle( RequestPasswordReset request, CancellationToken cancellationToken)
    {
        try
        {
            await authenticationService.SendPasswordResetLinkAsync(request.Email);
            return await Result<string>.SuccessAsync("If this email is registered, a password reset email has been sent.");
        }
        catch (Exception ex)
        {
            return await Result<string>.SuccessAsync(ex.Message);
        }
    }
} 