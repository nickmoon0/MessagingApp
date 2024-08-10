namespace MessagingApp.Domain.Common.Exceptions;

public class FailedToSendMessageException(string message) : Exception(message);