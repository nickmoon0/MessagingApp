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
        services.AddTransient<IHandler<CreateUserCommand, CreateUserResponse>, CreateUserHandler>();
        services.AddTransient<IHandler<RetrieveUserQuery, RetrieveUserResponse?>, RetrieveUserHandler>();
        services.AddTransient<IHandler<AuthenticateUserQuery, string>, AuthenticateUserHandler>();
        services
            .AddTransient<IHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>,
                CreateFriendRequestHandler>();
        
        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<RetrieveUserQuery>, ValidateRetrieveUserQuery>();
        
        return services;
    }
}