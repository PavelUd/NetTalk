using Application.Common.Interfaces.Repositories.Query;
using Application.Interfaces.Repositories;
using Application.Queries.QueryModels;
using MongoDB.Driver;

namespace Persistence.Repositories.Read;

internal class ChatReadOnlyRepository(IReadDbContext readDbContext) : BaseReadOnlyRepository<ChatQueryModel, int>(readDbContext), IChatReadOnlyRepository
{
    public async Task<IEnumerable<ChatQueryModel>> GetAllAsync()
    {
        using var asyncCursor = await Collection.FindAsync<ChatQueryModel>(Builders<ChatQueryModel>.Filter.Empty);
        return await asyncCursor.ToListAsync();
    }
}