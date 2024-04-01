using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.RespondToFriendRequest;

public class RespondToFriendRequestCommand
{
    public required Guid FriendRequestId { get; init; }
    public required Guid RespondingUserId { get; init; }
    public required FriendRequestStatus Response { get; init; }
}