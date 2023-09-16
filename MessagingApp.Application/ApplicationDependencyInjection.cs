using MessagingApp.Application;
using MessagingApp.Application.Commands;
using MessagingApp.Application.Handlers;
using MessagingApp.Application.Interfaces;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        // Register handlers
        services.AddTransient<IHandler<CreateUserCommand, Guid>, CreateUserHandler>();
        
        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        return services;
    }
}