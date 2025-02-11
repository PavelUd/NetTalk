using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories.Query;
using MongoDB.Driver;

namespace Persistence.Repositories.Read;

internal abstract class BaseReadOnlyRepository<TQueryModel, TKey>(IReadDbContext context) : IReadOnlyRepository
    where TQueryModel : class, IQueryModel
    where TKey : IEquatable<TKey>
{
    protected readonly IMongoCollection<TQueryModel> Collection = context.GetCollection<TQueryModel>();
    
    public async Task<TQueryModel> GetByIdAsync(TKey id)
    {
        using var asyncCursor = await Collection.FindAsync(queryModel => queryModel.Id.Equals(id));
        return await asyncCursor.FirstOrDefaultAsync();
    }
}