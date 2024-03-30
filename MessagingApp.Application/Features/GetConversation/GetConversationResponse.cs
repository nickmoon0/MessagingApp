using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Features.GetConversation;

public class GetConversationResponse
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public ConversationType Type { get; init; }
    public required IEnumerable<UserSummaryResponse> Participants { get; init; }
}

public class UserSummaryResponse
{
    public required Guid Id { get; init; }
    public required string? Username { get; init; }

    public static UserSummaryResponse UserSummaryFromUser(User user)
    {
        return new UserSummaryResponse
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}