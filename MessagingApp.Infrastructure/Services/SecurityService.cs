using MessagingApp.Application.Common.Services;

namespace MessagingApp.Infrastructure.Services;

public class SecurityService : ISecurityService
{
    public bool PasswordsMatch(string? password, string? hashedPassword)
    {
        if (password == null || hashedPassword == null) return false;
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }


    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}