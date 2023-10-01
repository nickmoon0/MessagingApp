using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }

    public Guid SendingUserId { get; set; }
    public User SendingUser { get; set; } = null!;
    
    public Guid ReceivingUserId { get; set; }
    public User ReceivingUser { get; set; } = null!;

    public string Text { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}