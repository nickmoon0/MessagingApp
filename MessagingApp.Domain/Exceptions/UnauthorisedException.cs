namespace MessagingApp.Domain.Exceptions;

public class UnauthorisedException : DomainException
{
    public UnauthorisedException(string message, string code) : base(message, code) { }
}