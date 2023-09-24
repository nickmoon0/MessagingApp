namespace MessagingApp.Application.FriendRequests.Commands.CreateFriendRequest;

public class CreateFriendRequestRequest
{
    public required Guid FromUser { get; set; }
    public required Guid ToUser { get; set; }
}