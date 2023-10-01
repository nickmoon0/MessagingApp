namespace MessagingApp.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public string Code { get; private set; }

    protected DomainException(string message, string code) : base(message)
    {
        Code = code;
    }
}