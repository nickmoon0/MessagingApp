using FluentValidation;
using MessagingApp.Application.Commands;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Validators;
using MessagingApp.Application.Handlers;
using MessagingApp.Application.Queries;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        // Register handlers
        services.AddTransient<IHandler<CreateUserCommand, Guid>, CreateUserHandler>();
        services.AddTransient<IHandler<RetrieveUserQuery, RetrieveUserDto?>, RetrieveUserHandler>();
        
        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<RetrieveUserQuery>, ValidateRetrieveUserQuery>();
        services.AddTransient<IValidator<CreateUserCommand>, ValidateCreateUserCommand>();
        
        return services;
    }
}