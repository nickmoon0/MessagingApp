namespace MessagingApp.Application.Common.Contracts;

public class SendMessageRequest
{
    public required string Text { get; set; }
    
    public required Guid SendingUserId { get; set; }
    public required Guid ReceivingUserId { get; set; }
}