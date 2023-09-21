namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}