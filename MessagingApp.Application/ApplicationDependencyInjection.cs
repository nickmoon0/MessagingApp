using FluentValidation;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.AcceptFriendRequest;
using MessagingApp.Application.Users.Commands.CreateFriendRequest;
using MessagingApp.Application.Users.Commands.CreateUser;
using MessagingApp.Application.Users.Commands.SendMessage;
using MessagingApp.Application.Users.Queries.AuthenticateUser;
using MessagingApp.Application.Users.Queries.GetMessageById;
using MessagingApp.Application.Users.Queries.RetrieveConversation;
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
        
        // Register friend request handlers
        services.AddTransient<IHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>,
            CreateFriendRequestHandler>();
        services.AddTransient<IHandler<AcceptFriendRequestCommand, AcceptFriendRequestResponse>,
            AcceptFriendRequestHandler>();
        
        // Register messaging handlers
        services.AddTransient<IHandler<SendMessageCommand, SendMessageResponse>, SendMessageHandler>();
        services.AddTransient<IHandler<GetMessageByIdQuery, GetMessageResponse>, GetMessageByIdHandler>();
        services
            .AddTransient<IHandler<RetrieveConversationQuery, RetrieveConversationResponse>,
                RetrieveConversationHandler>();
        
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