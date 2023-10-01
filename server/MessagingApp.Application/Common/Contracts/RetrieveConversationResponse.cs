using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Contracts;

public class RetrieveConversationResponse
{
    public Guid SendingUserId { get; set; }
    public Guid ReceivingUserId { get; set; }

    public IEnumerable<ConversationMessage> SentMessages { get; set; } = null!;
    public IEnumerable<ConversationMessage> ReceivedMessages { get; set; } = null!;

    public RetrieveConversationResponse(Guid sendingUserId, Guid receivingUserId, 
        IEnumerable<Message> messages)
    {
        SendingUserId = sendingUserId;
        ReceivingUserId = receivingUserId;
        
        ParseMessages(messages);
    }

    private void ParseMessages(IEnumerable<Message> messages)
    {
        var sentMessagesList = new List<ConversationMessage>();
        var receivedMessagesList = new List<ConversationMessage>();

        foreach (var message in messages)
        {
            var convMessage = new ConversationMessage(message.Text, message.Timestamp);
            if (message.SendingUserId == SendingUserId)
            {
                sentMessagesList.Add(convMessage);
            }
            else
            {
                receivedMessagesList.Add(convMessage);
            }
        }

        SentMessages = sentMessagesList;
        ReceivedMessages = receivedMessagesList;
    }
    
    public class ConversationMessage
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public ConversationMessage(string text, DateTime timestamp)
        {
            Text = text;
            Timestamp = timestamp;
        }
    }
}