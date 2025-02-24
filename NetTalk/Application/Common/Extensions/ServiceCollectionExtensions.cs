using System.Reflection;
using Application.Behaviors;
using Application.Chat.Dto;
using Application.Common.Interfaces;
using Application.Queries.Chat;
using Application.Stories;
using Domain.Events.Chat;
using Domain.Events.User;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper();
        services.AddMediator();
        services.AddStories();
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static void AddMediator(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(IApplicationMarker));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(ChatCreatedEvent)));
            cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(UserCreatedEvent)));
//            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        });
        
    }

    private static void AddStories(this IServiceCollection services)
    {
        services.AddScoped<CreateChatStory>();
        services.AddScoped<IStoryResolver, StoryResolver>();
    }

}