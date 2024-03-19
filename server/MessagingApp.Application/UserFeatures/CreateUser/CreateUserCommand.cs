using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.UserFeatures.CreateUser;

public class CreateUserCommand : IRequest<Result<CreateUserResponse>>
{
    public string? Username { get; init; }
    public string? Password { get; init; }
    
    public CreateUserCommand(CreateUserRequest createUserRequest)
    {
        Username = createUserRequest.Username;
        Password = createUserRequest.Password;
    }
}