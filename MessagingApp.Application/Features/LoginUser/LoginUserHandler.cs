using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.Services;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.LoginUser;

public class LoginUserHandler : IHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IApplicationContext _applicationContext;
    private readonly ITokenService _tokenService;
    private readonly ISecurityService _securityService;
    
    public LoginUserHandler(IApplicationContext applicationContext, ITokenService tokenService, ISecurityService securityService)
    {
        _applicationContext = applicationContext;
        _tokenService = tokenService;
        _securityService = securityService;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request)
    {
        // Try to get username
        var user = await _applicationContext.Users.SingleOrDefaultAsync(x => x.Username == request.Username);
        if (user == null) return new FailedToRetrieveEntityException("User does not exist");

        var loginUser = user.LoginUser(request.Username,
            hashedPassword => _securityService.PasswordsMatch(request.Password, hashedPassword));
        if (!loginUser) return new InvalidCredentialsException("Password is incorrect");
        
        // Generate tokens
        var tokenSetResult = await _tokenService.RotateTokens(user);
        if (!tokenSetResult.IsOk) return new FailedToCreateEntityException("Could not generate tokens");

        // Return response
        var tokens = tokenSetResult.Value;
        return new LoginUserResponse { Tokens = tokens };
    }
}