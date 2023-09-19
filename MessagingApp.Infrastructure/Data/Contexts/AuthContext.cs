using MessagingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Contexts;

public class AuthContext : IdentityDbContext
{
    //public DbSet<User> Users { get; set; } = null!;
    
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

    /*protected override void OnModelCreating(ModelBuilder builder)
    {
        
        builder.Entity<User>(entity =>
        {
            // Ensure table is called User and not Users
            entity.ToTable(nameof(User));
            
            // Set primary key
            entity.HasKey(x => x.Id);

            // Ensure usernames are unique
            entity.HasIndex(x => x.Username)
                .IsUnique();
        });

    }*/
}