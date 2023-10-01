namespace MessagingApp.Application.Common.Exceptions;

public class BadValuesException : Exception
{
    public BadValuesException(string message) : base(message) {}
}