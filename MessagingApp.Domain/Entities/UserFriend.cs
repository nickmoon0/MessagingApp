using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Domain.Entities;

public class UserFriend
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid FriendId { get; set; }
    public User Friend { get; set; } = null!;
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (UserFriend) obj;
        return UserId == other.UserId && FriendId == other.FriendId;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, FriendId);
    }
}