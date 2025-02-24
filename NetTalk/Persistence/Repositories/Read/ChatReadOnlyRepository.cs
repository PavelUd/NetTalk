using Application.Common.Interfaces.Repositories.Query;
using Application.Interfaces.Repositories;
using Application.Queries.QueryModels;
using MongoDB.Driver;

namespace Persistence.Repositories.Read;

internal class ChatReadOnlyRepository(IReadDbContext readDbContext) : BaseReadOnlyRepository<ChatQueryModel, Guid>(readDbContext), IChatReadOnlyRepository
{
}