using MessagingApp.Application.Common;
using MessagingApp.Application.Features.LoginUser;
using MessagingApp.Application.Features.RegisterUser;
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
        
        // Friend request handlers
        services.AddScoped<IHandler<SendFriendRequestCommand, SendFriendRequestResponse>, SendFriendRequestHandler>();
        
        return services;
    }
}