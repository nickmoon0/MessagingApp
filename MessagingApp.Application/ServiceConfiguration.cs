using MessagingApp.Application.Common;
using MessagingApp.Application.Features.RegisterUser;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingApp.Application;

public static class ServiceConfiguration
{
    public static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        services.AddScoped<IHandler<RegisterUserCommand, RegisterUserResponse>, RegisterUserHandler>();

        return services;
    }
}