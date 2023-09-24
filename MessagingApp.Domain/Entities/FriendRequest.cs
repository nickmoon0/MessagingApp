using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Entities;

public class FriendRequest
{
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    
    public DateTime RequestDate { get; set; }
    public FriendRequestStatus Status { get; set; }

    // Navigation properties
    public User FromUser { get; set; } = null!;
    public User ToUser { get; set; } = null!;
    
    public FriendRequest() { }

    public FriendRequest(Guid fromUserId, Guid toUserId, FriendRequestStatus status)
    {
        FromUserId = fromUserId;
        ToUserId = toUserId;
    }
}