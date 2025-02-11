using Application.Common.Result;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    public Task<Result<string>> Authenticate(string login, string password);
}