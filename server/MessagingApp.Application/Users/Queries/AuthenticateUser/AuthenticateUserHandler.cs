using System.Security.Authentication;
using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, Result<string>>
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    
    public AuthenticateUserHandler(IAuthRepository authRepository, ITokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }
    
    public async Task<Result<string>> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
    {
        // req.Username/Password cannot be null, AuthenticateUserRequest does not allow null values
        var user = new User 
        {
            Username = request.Username, 
            Password = request.Password
        };

        var userValid = await _authRepository.UserValid(user);
        user = await _authRepository.GetUserByUsername(user.Username);
        
        if (userValid)
        {
            // user cant be null if it is valid
            return new Result<string>(_tokenService.GenerateToken(user!));
        }
        
        var authException = new AuthenticationException("Invalid user credentials");
        return new Result<string>(authException);
    }
}