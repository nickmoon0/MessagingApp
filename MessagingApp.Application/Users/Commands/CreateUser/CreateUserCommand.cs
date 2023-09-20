using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CreateUserResponse>
{
    public string? Username { get; init; }
    public string? Password { get; init; }
    
    public CreateUserCommand(CreateUserDto createUserDto)
    {
        Username = createUserDto.Username;
        Password = createUserDto.Password;
    }
}