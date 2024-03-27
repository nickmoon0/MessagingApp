using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.GetFriendRequests;

public class GetFriendRequestsResponse
{
    public required ICollection<FriendRequestResponse> SentFriendRequests { get; init; }
    public required ICollection<FriendRequestResponse> ReceivedFriendRequests { get; init; }
}

public class FriendRequestResponse
{
    public Guid? FriendRequestId { get; init; }
    public ClientSafeUser? SendingUser { get; init; }
    public ClientSafeUser? ReceivingUser { get; init; }
    public FriendRequestStatus? Status { get; init; }

    public class ClientSafeUser
    {
        public Guid? UserId { get; init; }
        public string? Username { get; init; }
    }
    
    public static FriendRequestResponse FromFriendRequest(FriendRequest request)
    {
        return new FriendRequestResponse
        {
            FriendRequestId = request.Id,
            Status = request.Status,
            SendingUser = new ClientSafeUser
            {
                UserId = request.SendingUser?.Id,
                Username = request.SendingUser?.Username
            },
            ReceivingUser = new ClientSafeUser
            {
                UserId = request.ReceivingUser?.Id,
                Username = request.ReceivingUser?.Username
            }
        };
    }
}