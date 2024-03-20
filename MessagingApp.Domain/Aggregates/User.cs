using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class User : IDomainObject
{
    public Guid Id { get; set; }
    public bool Active { get; set; }

    public string? Username { get; set; }
    public string? HashedPassword { get; set; }
    public string? Bio { get; set; }

    public IEnumerable<FriendRequest> FriendRequests { get; set; } = [];
    public IEnumerable<Guid> Conversations { get; set; } = [];
}