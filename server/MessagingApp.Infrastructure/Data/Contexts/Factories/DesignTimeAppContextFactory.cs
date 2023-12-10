using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MessagingApp.Infrastructure.Data.Contexts.Factories;

public class DesignTimeAppContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        // Set up configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../", "MessagingApp.Api"))
            .AddJsonFile("appsettings.Local.json") // Ensure this file contains the connection string
            .Build();

        // Configure the DbContext
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        var connString = configuration.GetConnectionString("AppDb");
        
        optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));

        return new ApplicationContext(optionsBuilder.Options);
    }
}