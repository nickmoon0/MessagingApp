namespace MessagingApp.Application.Features.GetAllConversations;

public class GetAllConversationsQuery
{
    public required Guid UserId { get; init; }
}