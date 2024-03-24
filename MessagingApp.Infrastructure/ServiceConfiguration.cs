using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.Services;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Infrastructure.Services;
using MessagingApp.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingApp.Infrastructure;

public static class ServiceConfiguration
{
    public static IServiceCollection RegisterInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Create db context
        var dbConnString = configuration.GetConnectionString("MessagingAppDb");
        services.AddDbContext<ApplicationContext>(opt =>
            opt.UseMySql(dbConnString, ServerVersion.AutoDetect(dbConnString)));
        
        // Register individual Contexts (Ensure they use the same ApplicationContext instance per request)
        services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<ApplicationContext>());
        services.AddScoped<ITokenContext>(provider => provider.GetRequiredService<ApplicationContext>());

        // Register services
        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }

    public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        return services;
    }

}