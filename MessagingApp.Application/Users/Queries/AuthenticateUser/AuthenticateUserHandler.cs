using System.Security.Authentication;
using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserHandler : IHandler<AuthenticateUserQuery, string>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    
    public AuthenticateUserHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
    public async Task<Result<string>> Handle(AuthenticateUserQuery req)
    {
        var user = new User(req.Username, req.Password);

        var userValid = await _userRepository.UserValid(user);

        if (userValid)
        {
            return new Result<string>(_tokenService.GenerateToken(user));
        }
        
        var authException = new AuthenticationException("Invalid user credentials");
        return new Result<string>(authException);
    }
}