using MessagingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Contexts;

public class AuthContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Ensure table is called User and not Users
        builder.Entity<User>()
            .ToTable(nameof(User))
            .HasKey(x => x.Id);
    }
}