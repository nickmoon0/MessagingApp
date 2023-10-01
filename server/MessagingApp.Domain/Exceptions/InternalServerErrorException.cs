namespace MessagingApp.Domain.Exceptions;

public class InternalServerErrorException : DomainException
{
    public InternalServerErrorException(string message, string code) : base(message, code)
    {
    }
}