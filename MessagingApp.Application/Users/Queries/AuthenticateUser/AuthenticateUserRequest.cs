namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}