using MongoDB.Driver;

namespace Application.Common.Interfaces.Repositories.Query;

public interface IReadDbContext
{
    string ConnectionString { get; }

    /// <summary>
    /// Gets the collection for the specified query model.
    /// </summary>
    /// <typeparam name="TQueryModel">The type of the query model.</typeparam>
    /// <returns>The MongoDB collection for the specified query model.</returns>
    IMongoCollection<TQueryModel> GetCollection<TQueryModel>() where TQueryModel: class;
}