using MessagingApp.Application.Models;

namespace MessagingApp.Application.Features.RenewTokens;

public class RenewTokenResponse
{
    public required TokenSet Tokens { get; init; }
}