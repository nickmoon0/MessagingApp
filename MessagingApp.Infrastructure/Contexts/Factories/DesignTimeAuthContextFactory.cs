using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MessagingApp.Infrastructure.Contexts.Factories;

public class DesignTimeAuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        // Set up configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory() + "\\..\\MessagingApp.Api")
            .AddJsonFile("appsettings.Local.json") // Ensure this file contains the connection string
            .Build();

        // Configure the DbContext
        var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
        var connString = configuration.GetConnectionString("AuthDb");
        
        optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));

        return new AuthContext(optionsBuilder.Options);
    }
}