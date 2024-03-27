using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Features.GetFriends;

public class GetFriendsResponse
{
    public required IEnumerable<FriendsResponse> Friends { get; init; }
}

public class FriendsResponse
{
    public required Guid? UserId { get; init; }
    public required string? Username { get; init; }

    public static FriendsResponse FriendsResponseFromUser(User friend)
    {
        return new FriendsResponse
        {
            UserId = friend.Id,
            Username = friend.Username
        };
    }
}