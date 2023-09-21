namespace MessagingApp.Infrastructure.Data.Models;

public class FriendRequest
{
    public Guid ToUserId { get; set; }
    public Guid FromUserId { get; set; }
    public User ToUser { get; set; } = null!;
    public User FromUser { get; set; } = null!;
    
    public RequestStatus Status { get; set; } = null!;
}
