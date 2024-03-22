using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;

namespace MessagingApp.Domain.Entities;

public class Message : IPersistedObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }
    public string? Content { get; private set; }
    public DateTime TimeStamp { get; set; }

    public Conversation? MessageConversation { get; private set; }
    public User? SendingUser { get; private set; }
    
    private Message() { }

    private Message(User sendingUser, Conversation conversation, string content)
    {
        SendingUser = sendingUser;
        MessageConversation = conversation;
        Content = content;
        TimeStamp = DateTime.UtcNow;
        Active = true;
    }
    
    public static Result<Message, FailedToCreateEntityException> CreateMessage(
        User sendingUser, Conversation conversation, string content)
    {
        if (string.IsNullOrEmpty(content))
            return new FailedToCreateEntityException("Message content cannot be empty");

        var message = new Message(sendingUser, conversation, content);
        return message;
    }
    
}