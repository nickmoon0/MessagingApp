using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;

namespace MessagingApp.Domain.Entities;

public class FriendRequest : IDomainObject
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

    public static Result<FriendRequest, FailedToCreateEntityException> CreateFriendRequest(User sendingUser, User receivingUser,
        FriendRequestStatus status)
    {
        var friendRequest = new FriendRequest(sendingUser, receivingUser, status);
        return friendRequest;
    }
}
public enum FriendRequestStatus
{
    Accepted,
    Rejected,
    Pending
}