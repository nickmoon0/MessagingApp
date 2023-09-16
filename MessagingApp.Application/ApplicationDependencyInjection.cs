using MessagingApp.Application;
using MessagingApp.Application.Commands;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Handlers;

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