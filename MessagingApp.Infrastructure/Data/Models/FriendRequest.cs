namespace MessagingApp.Infrastructure.Data.Models;

public class FriendRequest
{
    public User ToUser { get; set; } = null!;
    public User FromUser { get; set; } = null!;
    
    public RequestStatus Status { get; set; } = null!;
}
