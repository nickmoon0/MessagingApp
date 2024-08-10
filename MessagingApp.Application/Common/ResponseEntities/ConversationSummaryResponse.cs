using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Common.ResponseEntities;

public class ConversationSummaryResponse
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public ConversationType Type { get; init; }

    public static ConversationSummaryResponse FromConversation(Conversation conversation)
    {
        return new ConversationSummaryResponse
        {
            Id = conversation.Id,
            Name = conversation.Name,
            Type = conversation.Type
        };
    }
}