namespace MessagingApp.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string message, string code) : base(message, code) { }
}