using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Models;
using MessagingApp.Infrastructure.Data.Repositories;
using MessagingApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register contexts
        var authConnString = configuration.GetConnectionString("AuthDb");
        var appConnString = configuration.GetConnectionString("AppDb");
        
        services.AddDbContext<AuthContext>(opt =>
            opt.UseMySql(authConnString, ServerVersion.AutoDetect(authConnString)));
        services.AddDbContext<ApplicationContext>(opt =>
            opt.UseMySql(appConnString, ServerVersion.AutoDetect(appConnString)));
        
        // Register identities
        services.AddIdentity<AuthUser, AuthRole>()
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();
        
        // Register repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IUserFriendRepository, UserFriendRepository>();
        
        // Register services
        services.AddSingleton<ITokenService, TokenService>();
        
        return services;
    }
}