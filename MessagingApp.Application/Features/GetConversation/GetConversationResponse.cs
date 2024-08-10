using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.GetConversation;

public class GetConversationResponse
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public ConversationType Type { get; init; }
    public required IEnumerable<UserSummaryResponse> Participants { get; init; }
    public required IEnumerable<MessageResponse> Messages { get; init; }
}