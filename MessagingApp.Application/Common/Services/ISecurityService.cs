namespace MessagingApp.Application.Common.Services;

public interface ISecurityService
{
    public bool PasswordsMatch(string? password, string? hashedPassword);
    public string HashPassword(string password);
}