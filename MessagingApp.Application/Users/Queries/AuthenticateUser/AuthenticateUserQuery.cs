using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserQuery : IRequest<string>
{
    public string? Username { get; init; }
    public string? Password { get; init; }

    public AuthenticateUserQuery(AuthenticateUserRequest authenticateUserRequest)
    {
        Username = authenticateUserRequest.Username;
        Password = authenticateUserRequest.Password;
    }
}