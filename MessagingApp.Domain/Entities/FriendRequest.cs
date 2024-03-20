using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Entities;

public class FriendRequest : IDomainObject
{
    public Guid Id { get; set; }
    public bool Active { get; set; }
    
    public Guid SendingUserId { get; set; }
    public Guid ReceivingUserId { get; set; }

    public FriendRequestStatus Status { get; set; }
}
public enum FriendRequestStatus
{
    Accepted,
    Rejected,
    Pending
}