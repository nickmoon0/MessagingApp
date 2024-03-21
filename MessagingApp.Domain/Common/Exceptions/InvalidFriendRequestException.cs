namespace MessagingApp.Domain.Common.Exceptions;

public class InvalidFriendRequestException(string message) : Exception(message)
{
}