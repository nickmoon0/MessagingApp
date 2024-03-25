using MessagingApp.Domain.Common;

namespace MessagingApp.Api.Common;

public static class Helpers
{
    public const string RefreshTokenName = "RefreshToken";
    public static IConfiguration Configuration { get; set; } = null!; // Injected in program.cs
    
    public static void AddRefreshTokenCookie(HttpContext context, string token)
    {
        var lifespanMinutes = Configuration.GetValue<int>("JwtSettings:RefreshTokenLife");
        // Create refresh token cookie (set to http-only)
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(lifespanMinutes)
        };
        context.Response.Cookies.Append(RefreshTokenName, token, cookieOptions);
    }
    
    public static Result<string> GetAccessToken(HttpContext context)
    {
        try
        {
            var token = context.Request.Headers.Authorization[0]!.Split(' ')[1];
            return token;
        }
        catch (Exception)
        {
            return new Exception("Failed to parse token");
        }
    }
}