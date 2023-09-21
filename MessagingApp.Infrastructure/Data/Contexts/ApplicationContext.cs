using MessagingApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Contexts;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RequestStatus> RequestStatuses { get; set; } = null!;
    public DbSet<UserFriend> UserFriends { get; set; } = null!;
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User table
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        modelBuilder.Entity<User>()
            .ToTable(nameof(User))
            .Property(user => user.Id).ValueGeneratedNever();
        
        // Configure Request Status Table
        modelBuilder.Entity<RequestStatus>()
            .ToTable(nameof(RequestStatus))
            .HasKey(status => status.Id);
        
        
        // Configure UserFriend entity
        modelBuilder.Entity<UserFriend>()
            .HasKey(uf => new { uf.UserId, uf.FriendId });

        modelBuilder.Entity<UserFriend>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevents cascading delete

        modelBuilder.Entity<UserFriend>()
            .HasOne(uf => uf.Friend)
            .WithMany()
            .HasForeignKey(uf => uf.FriendId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevents cascading delete
        
    }
}