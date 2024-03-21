namespace MessagingApp.Domain.Common;

public interface IDomainObject
{
    public Guid Id { get; }
    public bool Active { get; }
}