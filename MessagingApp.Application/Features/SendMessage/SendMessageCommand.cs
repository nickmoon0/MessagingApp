namespace MessagingApp.Application.Features.SendMessage;

public class SendMessageCommand
{
    public required Guid SendingUserId { get; init; }
    public required Guid ConversationId { get; init; }
    public required string Content { get; init; }
}