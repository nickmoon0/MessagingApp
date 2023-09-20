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
    private readonly IValidator<AuthenticateUserQuery> _validator;
    
    public AuthenticateUserHandler(IUserRepository userRepository, 
        ITokenService tokenService, 
        IValidator<AuthenticateUserQuery> validator)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _validator = validator;
    }
    
    public async Task<Result<string>> Handle(AuthenticateUserQuery req)
    {
        var valResult = await _validator.ValidateAsync(req);
        if (!valResult.IsValid)
        {
            var valException = new ValidationException(valResult.Errors);
            return new Result<string>(valException);
        }

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