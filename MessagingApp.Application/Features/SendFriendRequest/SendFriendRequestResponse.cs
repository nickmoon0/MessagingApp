using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.SendFriendRequest;

public class SendFriendRequestResponse
{
    public required Guid Id { get; init; }
    public required Guid SendingUserId { get; init; }
    public required Guid ReceivingUserId { get; init; }
}