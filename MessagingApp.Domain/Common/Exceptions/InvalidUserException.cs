namespace MessagingApp.Domain.Common.Exceptions;

public class InvalidUserException(string message) : Exception(message);