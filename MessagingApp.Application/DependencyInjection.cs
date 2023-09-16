using MessagingApp.Application;
using MessagingApp.Application.Commands;
using MessagingApp.Application.Handlers;
using MessagingApp.Application.Interfaces;
using MessagingApp.Domain.Entities;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        // Register handlers
        services.AddTransient<IHandler<CreateUserCommand, User>, CreateUserHandler>();
        
        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        return services;
    }
}