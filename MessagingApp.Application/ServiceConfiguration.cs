using MessagingApp.Application.Common;
using MessagingApp.Application.Features.GetAllConversations;
using MessagingApp.Application.Features.GetConversation;
using MessagingApp.Application.Features.GetFriendRequests;
using MessagingApp.Application.Features.GetFriends;
using MessagingApp.Application.Features.GetUser;
using MessagingApp.Application.Features.GetUserByName;
using MessagingApp.Application.Features.LoginUser;
using MessagingApp.Application.Features.RegisterUser;
using MessagingApp.Application.Features.RenewTokens;
using MessagingApp.Application.Features.RespondToFriendRequest;
using MessagingApp.Application.Features.SendFriendRequest;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingApp.Application;

public static class ServiceConfiguration
{
    public static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        // Auth handlers
        services.AddScoped<IHandler<RegisterUserCommand, RegisterUserResponse>, RegisterUserHandler>();
        services.AddScoped<IHandler<LoginUserCommand, LoginUserResponse>, LoginUserHandler>();
        services.AddScoped<IHandler<RenewTokenCommand, RenewTokenResponse>, RenewTokenHandler>();
        
        // Friend request handlers
        services
            .AddScoped<IHandler<RespondToFriendRequestCommand, RespondToFriendRequestResponse>,
                RespondToFriendRequestHandler>();
        services.AddScoped<IHandler<GetFriendRequestsQuery, GetFriendRequestsResponse>, GetFriendRequestsHandler>();
        
        // User handlers
        services.AddScoped<IHandler<SendFriendRequestCommand, SendFriendRequestResponse>, SendFriendRequestHandler>();
        services.AddScoped<IHandler<GetFriendsQuery, GetFriendsResponse>, GetFriendsHandler>();
        services.AddScoped<IHandler<GetUserQuery, GetUserResponse>, GetUserHandler>();
        services
            .AddScoped<IHandler<GetAllConversationsQuery, GetAllConversationsResponse>, GetAllConversationsHandler>();
        services.AddScoped<IHandler<GetUserByNameQuery, GetUserByNameResponse>, GetUserByNameHandler>();

        // Conversation handlers
        services.AddScoped<IHandler<GetConversationQuery, GetConversationResponse>, GetConversationHandler>();
        
        return services;
    }
}