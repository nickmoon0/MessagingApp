namespace MessagingApp.Application.Models;

public class TokenSet
{
    public RefreshToken? NewRefreshToken { get; set; }
    public string? NewAccessToken { get; set; }
}