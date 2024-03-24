using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;

namespace MessagingApp.Domain.Entities;

public class FriendRequest : IPersistedObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }
    public User? SendingUser { get; set; }
    public User? ReceivingUser { get; set; }

    public FriendRequestStatus Status { get; set; }
    private FriendRequest() {}

    private FriendRequest(User sendingUser, User receivingUser, FriendRequestStatus status)
    {
        SendingUser = sendingUser;
        ReceivingUser = receivingUser;
        Status = status;
        Active = true;
    }

    public static Result<FriendRequest> CreateFriendRequest(User sendingUser, User receivingUser)
    {
        if (sendingUser == receivingUser)
            return new FailedToCreateEntityException("User cannot send a friend request to themself");
        
        var friendRequest = new FriendRequest(sendingUser, receivingUser, FriendRequestStatus.Pending);
        return friendRequest;
    }
}
public enum FriendRequestStatus
{
    Accepted,
    Rejected,
    Pending
}