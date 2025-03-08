using DoomedDatabases.Postgres;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace IntegrationTest.Fixtures;

public class NetTalkWriteDbFixture : IDisposable
{
    public NetTalkDbContext DbContext { get; }
    private readonly ITestDatabase TestDatabase;

    public NetTalkWriteDbFixture()
    {
        TestDatabase = new TestDatabaseBuilder().WithConnectionString("Host=localhost;Port=5432;Database=net-talk-db;Username=postgres;Password=root").Build();
        TestDatabase.Create();
        
        var builder = new DbContextOptionsBuilder<NetTalkDbContext>();
        builder.UseNpgsql(TestDatabase.ConnectionString);
        DbContext = new NetTalkDbContext(builder.Options);
        DbContext.Database.EnsureCreated();
    }

    private bool _disposed;

    ~NetTalkWriteDbFixture () => Dispose(false);
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        
        if (disposing)
        {
            DbContext.Database.CloseConnection();
            DbContext.Database.EnsureDeleted();
            DbContext?.Dispose();
        }

        _disposed = true;
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollectionFixture : ICollectionFixture<NetTalkWriteDbFixture>
{
}