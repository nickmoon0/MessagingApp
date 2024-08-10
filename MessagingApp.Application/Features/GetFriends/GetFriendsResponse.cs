using MessagingApp.Application.Common.ResponseEntities;

namespace MessagingApp.Application.Features.GetFriends;

public class GetFriendsResponse
{
    public required IEnumerable<UserSummaryResponse> Friends { get; init; }
}