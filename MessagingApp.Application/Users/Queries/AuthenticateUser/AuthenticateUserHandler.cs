using System.Security.Authentication;
using LanguageExt.Common;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserHandler : IHandler<AuthenticateUserQuery, string>
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    
    public AuthenticateUserHandler(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
    
    public async Task<Result<string>> Handle(AuthenticateUserQuery req)
    {
        // req.Username/Password cannot be null, AuthenticateUserRequest does not allow null values
        var user = new User 
        {
            Username = req.Username, 
            Password = req.Password
        };

        var userValid = await _authService.UserValid(user);
        user = await _authService.GetUserByUsername(user.Username);
        
        if (userValid)
        {
            // user cant be null if it is valid
            return new Result<string>(_tokenService.GenerateToken(user!));
        }
        
        var authException = new AuthenticationException("Invalid user credentials");
        return new Result<string>(authException);
    }
}