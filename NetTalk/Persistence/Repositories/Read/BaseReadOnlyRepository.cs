using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories.Query;
using MongoDB.Driver;

namespace Persistence.Repositories.Read;

public abstract class BaseReadOnlyRepository<TQueryModel, TKey>(IReadDbContext context) : IReadOnlyRepository
    where TQueryModel : class, IQueryModel
    where TKey : IEquatable<TKey>
{
    protected readonly IMongoCollection<TQueryModel> Collection = context.GetCollection<TQueryModel>();
    
    public async Task<TQueryModel> GetByIdAsync(TKey id)
    {
        using var asyncCursor = await Collection.FindAsync(queryModel => queryModel.Id.Equals(id));
        return await asyncCursor.FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<TQueryModel>> GetAllAsync()
    {
        using var asyncCursor = await Collection.FindAsync<TQueryModel>(Builders<TQueryModel>.Filter.Empty);
        return await asyncCursor.ToListAsync();
    }
}