namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}