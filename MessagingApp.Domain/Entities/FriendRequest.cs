using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Entities;

public class FriendRequest : IDomainObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    private FriendRequest() {}

    private FriendRequest(User sendingUser, User receivingUser, FriendRequestStatus status)
    {
        SendingUser = sendingUser;
        ReceivingUser = receivingUser;
        Status = status;
        Active = true;
    }

    public static Result<FriendRequest, Exception> CreateFriendRequest(User sendingUser, User receivingUser,
        FriendRequestStatus status)
    {
        var friendRequest = new FriendRequest(sendingUser, receivingUser, status);
        return friendRequest;
    }
    
    public User SendingUser { get; set; }
    public User ReceivingUser { get; set; }

    public FriendRequestStatus Status { get; set; }
}
public enum FriendRequestStatus
{
    Accepted,
    Rejected,
    Pending
}