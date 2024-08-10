using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace MessagingApp.Application.Common.Contexts;

public interface IApplicationContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    public DbSet<User> Users { get; }
    public DbSet<Conversation> Conversations { get; }
    public DbSet<FriendRequest> FriendRequests { get; }
    public DbSet<Message> Messages { get; }
}