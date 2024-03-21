using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class Conversation : IDomainObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public IEnumerable<User> Participants { get; set; } = [];
    public IEnumerable<Message> Messages { get; set; } = [];
    
    private Conversation() {}
}