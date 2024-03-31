using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Common.ResponseEntities;

public class UserSummaryResponse
{
    public required Guid? Id { get; init; }
    public required string? Username { get; init; }

    public static UserSummaryResponse FromUser(User user)
    {
        return new UserSummaryResponse
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}