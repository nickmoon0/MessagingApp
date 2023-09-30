namespace MessagingApp.Application.Common.Contracts;

public class SendMessageResponse
{
    public Guid MessageId { get; set; }
    public string Text { get; set; }

    public SendMessageResponse(Guid messageId, string text)
    {
        MessageId = messageId;
        Text = text;
    }
}