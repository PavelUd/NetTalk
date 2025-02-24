using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Query;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using Persistence.Contexts;
using Persistence.Mappings;
using Persistence.Repositories;
using Persistence.Repositories.Read;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddReadDbContext(configuration);
        services.AddRepositories();
    }
    
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlConnection");

        services.AddDbContext<NetTalkDbContext>(options =>
            options.UseNpgsql(connectionString ));
        
    }

    public static void AddReadDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        
        services
            .AddScoped<ISynchronizeDb, ReadNetTalkDbContext>()
            .AddScoped<IReadDbContext, ReadNetTalkDbContext>()
            .AddScoped<ReadNetTalkDbContext>();

        ConfigureMongoDb();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient<IChatRepository, ChatRepository>()
            .AddTransient<IEventStoreRepository, EventStoreRepository>()
            .AddScoped<IChatReadOnlyRepository, ChatReadOnlyRepository>()
            .AddScoped<IMessageReadOnlyRepository, MessageReadOnlyRepository>()
            .AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
        

    }
    
    private static void ConfigureMongoDb()
    {
        try
        {
            
            BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
            ConventionRegistry.Register("Conventions",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention(), 
                    new EnumRepresentationConvention(BsonType.String), 
                    new IgnoreExtraElementsConvention(true), 
                    new IgnoreIfNullConvention(true) 
                }, _ => true);
            
            new ChatMap().Configure();
            new UserMap().Configure();
        }
        catch
        {
            // ignored
        }
    }
}