namespace MessagingApp.Infrastructure.Settings;

public class JwtSettings
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int AccessTokenLife { get; set; }
    public int RefreshTokenLife { get; set; }
    public int RefreshTokenLength { get; set; }
}