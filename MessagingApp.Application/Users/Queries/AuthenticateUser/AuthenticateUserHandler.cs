using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Common.Interfaces.Services;

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
    
    // TODO: Update so that credential verification occurs in repository
    public async Task<Result<string>> Handle(AuthenticateUserQuery req)
    {
        var valResult = await _validator.ValidateAsync(req);
        if (!valResult.IsValid)
        {
            var valException = new ValidationException(valResult.Errors);
            return new Result<string>(valException);
        }
        
        // Suppress warning as validation ensures its not null
        var user = await _userRepository.GetUserByUsername(req.Username!);

        if (user == null)
        {
            var userNotFoundEx = new UnauthorizedAccessException("User not found");
            return new Result<string>(userNotFoundEx);
        }

        var passwordMatch = BCrypt.Net.BCrypt.Verify(req.Password, user.HashedPassword);
        if (passwordMatch)
        {
            // Generate and return token
            try
            {
                var token = _tokenService.GenerateToken(user);
                return new Result<string>(token);
            }
            catch (MissingConfigException ex)
            {
                return new Result<string>(ex);
            }
        }
        
        var passwordsDontMatchEx = new UnauthorizedAccessException("Incorrect password");
        return new Result<string>(passwordsDontMatchEx);
    }
}