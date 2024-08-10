using MessagingApp.Application.Models;

namespace MessagingApp.Application.Features.LoginUser;

public class LoginUserResponse
{
    public required TokenSet Tokens { get; init; }
}