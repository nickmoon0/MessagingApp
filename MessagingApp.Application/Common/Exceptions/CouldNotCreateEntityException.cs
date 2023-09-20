namespace MessagingApp.Application.Common.Exceptions;

public class CouldNotCreateEntityException : Exception
{
    public CouldNotCreateEntityException(string message) : base(message) { }
}