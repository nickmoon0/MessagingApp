namespace MessagingApp.Domain.Common;

public interface IDomainObject
{
    public Guid Id { get; set; }
    public bool Active { get; set; }
}