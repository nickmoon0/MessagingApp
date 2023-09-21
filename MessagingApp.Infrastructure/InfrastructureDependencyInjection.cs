using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Entities.Security;
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
        
        services.AddDbContext<AuthContext>(opt =>
            opt.UseMySql(authConnString, ServerVersion.AutoDetect(authConnString)));
    
        // Register identities
        services.AddIdentity<AuthUser, AuthRole>()
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();
        
        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        
        // Register services
        services.AddSingleton<ITokenService, TokenService>();
        
        return services;
    }
}