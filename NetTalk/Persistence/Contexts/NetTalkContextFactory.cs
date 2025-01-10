using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts;

public class NetTalkContextFactory : IDesignTimeDbContextFactory<NetTalkDbContext>
{
    public NetTalkDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NetTalkDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=net-talk-db;Username=postgres;Password=root");
        
        return new NetTalkDbContext(optionsBuilder.Options);
    }
    
}