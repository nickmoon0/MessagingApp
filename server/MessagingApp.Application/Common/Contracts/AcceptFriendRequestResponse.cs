namespace MessagingApp.Application.Common.Contracts;

public class AcceptFriendRequestResponse
{
    public Guid FriendId { get; set; }
    public string FriendName { get; set; }

    public AcceptFriendRequestResponse(Guid friendId, string friendName)
    {
        FriendId = friendId;
        FriendName = friendName;
    }
}