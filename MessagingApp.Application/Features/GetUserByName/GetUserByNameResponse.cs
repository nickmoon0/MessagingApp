using MessagingApp.Application.Common.ResponseEntities;

namespace MessagingApp.Application.Features.GetUserByName;

public class GetUserByNameResponse
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string Bio { get; init; }
    public required IEnumerable<UserSummaryResponse> Friends { get; init; }
}

