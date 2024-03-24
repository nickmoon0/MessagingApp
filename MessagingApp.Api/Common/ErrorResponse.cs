namespace MessagingApp.Api.Common;

public class ErrorResponse(string errorMessage)
{
    public string ErrorMessage { get; init; } = errorMessage;
}