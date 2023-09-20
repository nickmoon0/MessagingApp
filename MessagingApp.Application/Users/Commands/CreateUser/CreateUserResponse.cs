namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserResponse
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
}