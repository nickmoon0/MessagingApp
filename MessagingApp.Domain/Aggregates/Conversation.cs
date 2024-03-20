using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class Conversation : IDomainObject
{
    public Guid Id { get; set; }
    public bool Active { get; set; }

    public IEnumerable<User> Participants { get; set; } = [];
    public IEnumerable<Message> Messages { get; set; } = [];
}