namespace MessagingApp.Application.Features.SendFriendRequest;

public class SendFriendRequestCommand
{
    public required Guid SendingUserId { get; set; }
    public required Guid ReceivingUserId { get; set; }
}