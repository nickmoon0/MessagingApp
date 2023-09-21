using System.Security.Authentication;
using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserHandler : IHandler<AuthenticateUserQuery, string>
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    
    public AuthenticateUserHandler(IAuthRepository authRepository, ITokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }
    
    public async Task<Result<string>> Handle(AuthenticateUserQuery req)
    {
        var user = new UserDto(req.Username, req.Password);

        var userValid = await _authRepository.UserValid(user);

        if (userValid)
        {
            return new Result<string>(_tokenService.GenerateToken(user));
        }
        
        var authException = new AuthenticationException("Invalid user credentials");
        return new Result<string>(authException);
    }
}