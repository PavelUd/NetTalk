using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories.Commands;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    
}