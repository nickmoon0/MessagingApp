using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Aggregates;

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
}