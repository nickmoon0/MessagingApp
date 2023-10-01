namespace MessagingApp.Application.Common.Contracts;

public class CreateFriendRequestRequest
{
    public required Guid FromUser { get; set; }
    public required Guid ToUser { get; set; }
}