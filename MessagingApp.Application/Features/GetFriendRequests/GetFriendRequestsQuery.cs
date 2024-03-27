using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.GetFriendRequests;

public class GetFriendRequestsQuery
{
    public required Guid UserId { get; init; }
    public required bool GetSentRequests { get; init; }
    public required bool GetReceivedRequests { get; init; }
    public required FriendRequestStatus? Status { get; init; }
}