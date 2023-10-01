namespace MessagingApp.Application.Common.Exceptions;

public class MissingConfigException : Exception
{
    public MissingConfigException(string message) : base (message) { }
}