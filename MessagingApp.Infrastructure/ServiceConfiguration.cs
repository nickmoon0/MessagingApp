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
        
        // Register individual Contexts
        services.AddTransient<IApplicationContext, ApplicationContext>();
        services.AddTransient<ITokenContext, ApplicationContext>();

        // Register services
        services.AddTransient<ITokenService, TokenService>();
        
        return services;
    }

    public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        return services;
    }

}