namespace MessagingApp.Domain.Common;

public interface IPersistedObject
{
    public Guid Id { get; }
    public bool Active { get; }
}