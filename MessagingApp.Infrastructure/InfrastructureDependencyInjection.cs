using MessagingApp.Application.Interfaces.Repositories;
using MessagingApp.Infrastructure.Contexts;
using MessagingApp.Infrastructure.Repositories;
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
    
        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}