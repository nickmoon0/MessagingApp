using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CreateUserResponse>
{
    public string? Username { get; init; }
    public string? Password { get; init; }
    
    public CreateUserCommand(CreateUserRequest createUserRequest)
    {
        Username = createUserRequest.Username;
        Password = createUserRequest.Password;
    }
}