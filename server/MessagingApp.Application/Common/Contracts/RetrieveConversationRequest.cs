namespace MessagingApp.Application.Common.Contracts;

public class RetrieveConversationRequest
{
    public Guid RequestingUser { get; set; }
    public Guid ConversationUserId { get; set; }
}