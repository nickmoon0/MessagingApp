using MessagingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Test.Infrastructure;

public static class DatabaseSetup
{
    /// <summary>
    /// Creates a new in-memory database each time its called
    /// </summary>
    public static DbContextOptions<ApplicationContext> Options =>
        new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: $"MessagingApp_{Guid.NewGuid()}")
            .Options;
}