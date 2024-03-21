namespace MessagingApp.Domain.Common.Exceptions;

public class InvalidConversationException(string message) : Exception(message);