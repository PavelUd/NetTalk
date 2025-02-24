using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Persistence.Contexts;

public sealed class ReadNetTalkDbContext: IReadDbContext, ISynchronizeDb
{

    #region Constructor

    private const string DatabaseName = "NetTalk";
    private const int RetryCount = 2;

    private static readonly ReplaceOptions DefaultReplaceOptions = new()
    {
        IsUpsert = true
    };

    private static readonly CreateIndexOptions DefaultCreateIndexOptions = new()
    {
        Unique = true,
        Sparse = true
    };

    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    

    public ReadNetTalkDbContext(IOptions<ConnectionOptions> options)
    {
        ConnectionString = options.Value.NoSqlConnection;

        _mongoClient = new MongoClient(options.Value.NoSqlConnection);
        _mongoDatabase = _mongoClient.GetDatabase(DatabaseName);
    }

    #endregion

    #region IReadDbContext
    public string ConnectionString { get; }
    
    public IMongoCollection<TQueryModel> GetCollection<TQueryModel>() where TQueryModel : class =>
        _mongoDatabase.GetCollection<TQueryModel>(typeof(TQueryModel).Name);
    
    
    #endregion
    
    #region ISynchronizeDb

    public async Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upsertFilter)
        where TQueryModel : class, IQueryModel
    {
        var collection = GetCollection<TQueryModel>();
        await collection.ReplaceOneAsync(upsertFilter, queryModel, DefaultReplaceOptions);
    }
    
    #endregion
    
    #region IDisposable
    
    private bool _disposed;
    
    ~ReadNetTalkDbContext() => Dispose(false);
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        
        if (disposing)
            _mongoClient.Dispose();

        _disposed = true;
    }

    #endregion
}