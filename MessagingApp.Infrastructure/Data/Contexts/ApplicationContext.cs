using MessagingApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Contexts;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<FriendRequest> FriendRequests { get; set; } = null!;
    public DbSet<RequestStatus> RequestStatuses { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User table
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        modelBuilder.Entity<User>()
            .ToTable(nameof(User))
            .Property(user => user.Id).ValueGeneratedNever();

        // Configuration for sent friend requests
        modelBuilder.Entity<User>()
            .HasMany(u => u.SentFriendRequests)
            .WithOne(fr => fr.FromUser)
            .HasForeignKey(fr => fr.FromUserId);

        // Configuration for received friend requests
        modelBuilder.Entity<User>()
            .HasMany(u => u.ReceivedFriendRequests)  
            .WithOne(fr => fr.ToUser)
            .HasForeignKey(fr => fr.ToUserId);
        
        // Configure Request Status Table
        modelBuilder.Entity<RequestStatus>()
            .ToTable(nameof(RequestStatus))
            .HasKey(status => status.Id);
        
        // Configure Friend Request Table
        modelBuilder.Entity<FriendRequest>()
            .ToTable(nameof(FriendRequest))
            .HasKey(req => new { req.ToUser, req.FromUser });
    }
}