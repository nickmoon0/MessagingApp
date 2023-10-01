using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Contracts;

public class GetMessageResponse
{
    public Guid MessageId { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }

    public GetMessageResponse(Message message)
    {
        MessageId = message.Id;
        FromUserId = message.SendingUserId;
        ToUserId = message.ReceivingUserId;
        Text = message.Text;
        Timestamp = message.Timestamp;
    }
}