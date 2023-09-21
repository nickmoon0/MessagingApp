namespace MessagingApp.Infrastructure.Data.Models;

public class User
{
    public Guid Id { get; set; }
    
    public List<UserFriend> Friends { get; set; } = null!;
    
    public List<FriendRequest> SentFriendRequests { get; set; } = null!;
    public List<FriendRequest> ReceivedFriendRequests { get; set; } = null!;
}