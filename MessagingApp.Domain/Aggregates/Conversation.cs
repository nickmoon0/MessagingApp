using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class Conversation : IPersistedObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public ICollection<User> Participants { get; private set; } = [];
    public ICollection<Message> Messages { get; private set; } = [];
    
    public string? Name { get; init; }
    public ConversationType Type { get; init; }
    
    private Conversation() {}

    private Conversation(ICollection<User> participants, ConversationType type, string? name = null)
    {
        Participants = participants;
        Type = type;
        Name = name;
        Active = true;
    }

    public static Result<Conversation> CreateDirectMessage(User user1, User user2)
    {
        // Check that users are not the same
        if (user1.Id == user2.Id)
            return new FailedToCreateEntityException("User cannot create conversation with themselves");
        
        // Check that users are friends
        if (!user1.Friends.Contains(user2) || !user2.Friends.Contains(user1))
            return new FailedToCreateEntityException("Users are not friends");
        
        var participants = new List<User> { user1, user2 };
        var conversation = new Conversation(participants, ConversationType.DirectMessage);
        
        return conversation;
    }

    public Result<Message> SendMessage(User sendingUser, string content)
    {
        // Check all requirements to send a message are met
        if (!Active) return new FailedToSendMessageException("Conversation is not active");
        if (!Participants.Contains(sendingUser)) return new FailedToSendMessageException("User must be part of conversation to send a message");

        // Create message and check that it was created successfully
        var messageResult = Message.CreateMessage(sendingUser, this, content);
        if (!messageResult.IsOk) return new FailedToSendMessageException(messageResult.Error.Message);

        // Add message to conversation
        var message = messageResult.Value;
        Messages.Add(message);
        
        return message;
    }
}

public enum ConversationType
{
    DirectMessage,
    GroupChat
}