using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.UserFeatures.AuthenticateUser;

public class AuthenticateUserQuery : IRequest<Result<string>>
{
    public string Username { get; init; }
    public string Password { get; init; }

    public AuthenticateUserQuery(AuthenticateUserRequest authenticateUserRequest)
    {
        Username = authenticateUserRequest.Username;
        Password = authenticateUserRequest.Password;
    }
}