namespace MessagingApp.Application.Common.Contracts;

public class SendMessageResponse
{
    public Guid MessageId { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }

    public SendMessageResponse(Guid messageId, string text, DateTime timestamp)
    {
        MessageId = messageId;
        Text = text;
        Timestamp = timestamp;
    }
}