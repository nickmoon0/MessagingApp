using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class Conversation : IDomainObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public IEnumerable<User> Participants { get; set; } = [];
    public IEnumerable<Message> Messages { get; set; } = [];
    
    private Conversation() {}

    private Conversation(IEnumerable<User> participants)
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
}