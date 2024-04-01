using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.RespondToFriendRequest;

public class RespondToFriendRequestResponse
{
    public required Guid FriendRequestId { get; init; }
    public required FriendRequestStatus NewStatus { get; init; }
}