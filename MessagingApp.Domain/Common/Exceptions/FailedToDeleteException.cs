namespace MessagingApp.Domain.Common.Exceptions;

public class FailedToDeleteException(string message) : Exception(message)
{
    
}