using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Models;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options), 
    IApplicationContext, ITokenContext
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<Conversation> Conversations { get; init; } = null!;
    public DbSet<FriendRequest> FriendRequests { get; init; } = null!;
    public DbSet<Message> Messages { get; init; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure table names are singular
        modelBuilder.Entity<User>().ToTable(nameof(User));
        modelBuilder.Entity<Conversation>().ToTable(nameof(Conversation));
        modelBuilder.Entity<FriendRequest>().ToTable(nameof(FriendRequest));
        modelBuilder.Entity<Message>().ToTable(nameof(Message));
        modelBuilder.Entity<RefreshToken>().ToTable(nameof(RefreshToken));
        
        // User to FriendRequests sent and received (One-to-Many, Two Ways)
        modelBuilder.Entity<FriendRequest>()
            .HasOne(friendReq => friendReq.SendingUser)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FriendRequest>()
            .HasOne(friendReq => friendReq.ReceivingUser)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        // User to Conversations (Many-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(user => user.Conversations)
            .WithMany(conversation => conversation.Participants)
            .UsingEntity<Dictionary<string, object>>(
                "UserConversation",
                joinRight => joinRight.HasOne<Conversation>()
                    .WithMany()
                    .HasForeignKey("ConversationId"),
                joinLeft => joinLeft.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
            );
        
        // User to Friends (Many-to-Many)
        // This requires a bit of creativity since EF Core handles many-to-many with a join entity implicitly.
        modelBuilder.Entity<User>()
            .HasMany(user => user.Friends)
            .WithMany()
            .UsingEntity(join => join.ToTable("UserFriend")); // Specify the join table
    }

}