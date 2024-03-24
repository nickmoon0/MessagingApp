using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.Services;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.RegisterUser;

public class RegisterUserHandler : IHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IApplicationContext _applicationContext;
    private readonly ITokenService _tokenService;
    private readonly ISecurityService _securityService;
    public RegisterUserHandler(IApplicationContext applicationContext, ITokenService tokenService, ISecurityService securityService)
    {
        _applicationContext = applicationContext;
        _tokenService = tokenService;
        _securityService = securityService;
    }
    
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request)
    {
        // Prevent creation of user if username taken
        var existingUsers = await _applicationContext.Users
            .AnyAsync(x => string.Equals(x.Username!.ToLower(), request.Username.ToLower()));
        if (existingUsers) return new FailedToCreateEntityException("Username has been taken");
        
        // Attempt to create new user
        var userResult = User.CreateNewUser(request.Username, request.Password, request.Bio, _securityService.HashPassword);
        if (!userResult.IsOk) return userResult.Error; // Domain rules failed

        // Create user in database
        var user = userResult.Value;
        await _applicationContext.Users.AddAsync(user);
        await _applicationContext.SaveChangesAsync();

        // Create new tokens to return to client
        var tokenSetResult = await _tokenService.RotateTokens(user);
        if (!tokenSetResult.IsOk) return tokenSetResult.Error;

        var tokenSet = tokenSetResult.Value;
        
        return new RegisterUserResponse
        {
            Id = user.Id,
            Username = user.Username!,
            Tokens = tokenSet,
            Bio = user.Bio
        };
    }
}