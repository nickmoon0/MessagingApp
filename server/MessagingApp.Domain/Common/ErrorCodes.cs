namespace MessagingApp.Domain.Common;

/// <summary>
/// Constant error codes which can be returned and mapped to exceptions
/// </summary>
public static class ErrorCodes
{
    public const string NotFound = nameof(NotFound);
    public const string BadRequest = nameof(BadRequest);
    public const string Unauthorised = nameof(Unauthorised);
    public const string InternalServerError = nameof(InternalServerError);
}