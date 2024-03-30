using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Features.GetAllConversations;

public class GetAllConversationsResponse
{
    public required IEnumerable<ConversationResponse> Conversations { get; init; }
}

public class ConversationResponse
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public ConversationType Type { get; init; }

    public static ConversationResponse ConversationResponseFromConversation(Conversation conversation)
    {
        return new ConversationResponse
        {
            Id = conversation.Id,
            Name = conversation.Name,
            Type = conversation.Type
        };
    }
}