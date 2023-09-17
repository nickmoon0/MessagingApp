namespace MessagingApp.Application.Common.Exceptions;

public class NotEnoughDetailsException : Exception
{
    public NotEnoughDetailsException(string message) : base(message) { }
}