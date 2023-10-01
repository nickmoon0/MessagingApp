namespace MessagingApp.Domain.Exceptions;

public class BadRequestException : DomainException
{
    public BadRequestException(string message, string code) : base(message, code) { }
}