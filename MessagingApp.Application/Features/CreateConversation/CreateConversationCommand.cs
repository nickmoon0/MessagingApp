using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Features.CreateConversation;

public class CreateConversationCommand
{
    public required Guid RequestingUserId { get; init; }
    public required ConversationType Type { get; init; }
    public required IEnumerable<Guid> ParticipantIds { get; init; }
}