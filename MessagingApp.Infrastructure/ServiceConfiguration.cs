using MessagingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingApp.Infrastructure;

public static class ServiceConfiguration
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnString = configuration.GetConnectionString("MessagingAppDb");
        services.AddDbContext<ApplicationContext>(opt =>
            opt.UseMySql(dbConnString, ServerVersion.AutoDetect(dbConnString)));
        
        return services;
    }
}