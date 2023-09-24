using MessagingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Contexts;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<FriendRequest> FriendRequests { get; set; } = null!;
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary keys
        modelBuilder.Entity<User>()
            .Ignore(u => u.Password)
            .Ignore(u => u.HashedPassword)
            .ToTable(nameof(User))
            .HasKey(u => u.Id);

        modelBuilder.Entity<FriendRequest>()
            .ToTable(nameof(FriendRequest))
            .HasKey(fr => fr.Id);

        // Define relationships for FriendRequest
        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.FromUser)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.FromUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.ToUser)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(fr => fr.ToUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        
        modelBuilder.Entity<UserFriend>()
            .ToTable(nameof(UserFriend))
            .HasKey(uf => new { uf.UserId, uf.FriendId }); // Composite key

        modelBuilder.Entity<UserFriend>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Restrict); // To prevent cascade delete

        modelBuilder.Entity<UserFriend>()
            .HasOne(uf => uf.Friend)
            .WithMany()
            .HasForeignKey(uf => uf.FriendId)
            .OnDelete(DeleteBehavior.Restrict); // To prevent cascade delete
    }
}