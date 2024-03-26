namespace MessagingApp.Application.Features.RenewTokens;

public class RenewTokenCommand
{
    public required string RefreshToken { get; init; }
}