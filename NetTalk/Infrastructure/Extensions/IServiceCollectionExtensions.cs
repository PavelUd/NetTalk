using Application.Interfaces;
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
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

}