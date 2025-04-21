using Application.Common.Interfaces;
using Application.Interfaces;
using Infrastructure.Encryption;
using Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddServices(configuration);
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IEmailService, EmailService>(s => new EmailService(configuration));
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ISymmetricKeyEncryptor, SymmetricKeyEncryptor>();
        services.AddScoped<IMessageEncryptor, MessageEncryptor>();
    }

}