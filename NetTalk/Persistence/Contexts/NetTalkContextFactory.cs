using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts;

public class NetTalkContextFactory : IDesignTimeDbContextFactory<NetTalkContext>
{
    public NetTalkContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NetTalkContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=net-talk-db;Username=postgres;Password=root");
        
        return new NetTalkContext(optionsBuilder.Options);
    }
    
}