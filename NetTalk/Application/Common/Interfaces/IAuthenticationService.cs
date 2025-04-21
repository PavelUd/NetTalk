using Application.Common.Result;
using Domain.Entities;
using Infrastructure.Identity.Models;

namespace Application.Common.Interfaces;

public interface IAuthenticationService
{
    public Task<Result<TokenPair>> Authenticate(string login, string password);
    public Task<Result<TokenPair>> ConfirmRegistrationAsync(Guid registrationId, long code);
    public Task SendPasswordResetLinkAsync(string email);
    public Task<Result<Registration>> RegisterAsync(string login, string password, string email);
    public Task<Result<TokenPair>> Authenticate(string refreshToken);
    public Task<Result<string>> ResetPasswordAsync(string token, string newPassword);

}