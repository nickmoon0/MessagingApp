using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingApp.Infrastructure.Data.Models;

public class FriendRequest
{
    public Guid ToUserId { get; set; }
    public Guid FromUserId { get; set; }
    
    [ForeignKey(nameof(ToUserId))]
    public User ToUser { get; set; } = null!;
    [ForeignKey(nameof(FromUserId))]
    public User FromUser { get; set; } = null!;
    
    public RequestStatus Status { get; set; } = null!;
}
