namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}