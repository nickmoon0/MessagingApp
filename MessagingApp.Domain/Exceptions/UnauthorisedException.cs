namespace MessagingApp.Domain.Exceptions;

public class UnauthorisedException : Exception
{
    public UnauthorisedException(string message) : base (message) { }
}