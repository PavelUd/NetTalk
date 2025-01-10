using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    public Task<string> Authenticate(User user);
}