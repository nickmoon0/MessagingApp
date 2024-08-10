namespace MessagingApp.Application.Features.GetConversation;

public class GetConversationQuery
{
    public required Guid UserId { get; init; }
    public required Guid ConversationId { get; init; }
    public int? MessagesToRetrieve { get; init; }
}