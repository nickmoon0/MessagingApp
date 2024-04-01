namespace MessagingApp.Application.Features.RegisterUser;

public class RegisterUserCommand
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public string? Bio { get; init; }
}