using Application.Common.Interfaces.Repositories.Commands;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.Write;

public class RefreshTokenRepository(NetTalkDbContext context)
    : BaseRepository<RefreshToken>(context), IRefreshTokenRepository;