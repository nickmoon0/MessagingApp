using MessagingApp.Application.Common.ResponseEntities;

namespace MessagingApp.Application.Features.GetFriendRequests;

public class GetFriendRequestsResponse
{
    public required ICollection<FriendRequestResponse> SentFriendRequests { get; init; }
    public required ICollection<FriendRequestResponse> ReceivedFriendRequests { get; init; }
}