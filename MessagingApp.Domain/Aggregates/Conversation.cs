using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class Conversation : IDomainObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public ICollection<User> Participants { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    
    private Conversation() {}

    private Conversation(ICollection<User> participants)
    {
        Participants = participants;
        Active = true;
    }

    public static Result<Conversation, InvalidConversationException> CreateDirectMessage(User user1, User user2)
    {
        // Check that users are not the same
        if (user1.Id == user2.Id)
            return new InvalidConversationException("User cannot create conversation with themselves");
        
        var participants = new List<User> { user1, user2 };
        var conversation = new Conversation(participants);
        
        return conversation;
    }

    public Result<Message, FailedToSendMessageException> SendMessage(User sendingUser, string content)
    {
        if (string.IsNullOrEmpty(content)) return new FailedToSendMessageException("Message content cannot be empty");
        
        if (!Active) return new FailedToSendMessageException("Conversation is not active");
        if (!Participants.Contains(sendingUser)) return new FailedToSendMessageException("User must be part of conversation to send a message");

        var message = new Message()
        {
            Content = content,
            TimeStamp = DateTime.UtcNow,
            MessageConversation = this,
            SendingUser = sendingUser
        };
        
        Messages.Add(message);
        
        return message;
    }
}