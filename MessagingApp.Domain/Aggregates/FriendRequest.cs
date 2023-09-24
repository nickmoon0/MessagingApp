namespace MessagingApp.Domain.Aggregates;

public class FriendRequest
{
    public Guid ToUser { get; set; }
    public Guid FromUser { get; set; }

    public FriendRequest(Guid toUser, Guid fromUser)
    {
        ToUser = toUser;
        FromUser = fromUser;
    }
}