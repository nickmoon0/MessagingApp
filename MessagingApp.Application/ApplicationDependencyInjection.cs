using FluentValidation;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.FriendRequests.Commands.CreateFriendRequest;
using MessagingApp.Application.Users.Commands.CreateUser;
using MessagingApp.Application.Users.Queries.AuthenticateUser;
using MessagingApp.Application.Users.Queries.RetrieveUser;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        // Register user handlers
        AddUserHandlers(services);
        AddFriendRequestHandlers(services);
        
        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<RetrieveUserQuery>, ValidateRetrieveUserQuery>();
        
        return services;
    }

    private static IServiceCollection AddUserHandlers(IServiceCollection services)
    {
        services.AddTransient<IHandler<CreateUserCommand, CreateUserResponse>, CreateUserHandler>();
        services.AddTransient<IHandler<RetrieveUserQuery, RetrieveUserResponse?>, RetrieveUserHandler>();
        services.AddTransient<IHandler<AuthenticateUserQuery, string>, AuthenticateUserHandler>();
        return services;
    }

    private static IServiceCollection AddFriendRequestHandlers(IServiceCollection services)
    {
        services
            .AddTransient<IHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>,
                CreateFriendRequestHandler>();

        return services;
    }
}