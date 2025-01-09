using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
    }
    
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddDbContext<NetTalkContext>(options =>
            options.UseNpgsql(connectionString ));
    }

}