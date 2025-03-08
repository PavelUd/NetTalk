using DoomedDatabases.Postgres;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Xunit;

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

    public void Dispose()
    {
        TestDatabase.Drop();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollectionFixture : ICollectionFixture<NetTalkWriteDbFixture>
{
}