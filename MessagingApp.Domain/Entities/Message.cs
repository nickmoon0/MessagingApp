using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Entities;

public class Message : IDomainObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public string? Content { get; set; }
    public DateTime TimeStamp { get; set; }

    public Conversation? MessageConversation { get; set; }
    public User? SendingUser { get; set; }
}