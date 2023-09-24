using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly ApplicationContext _applicationContext;
    public AuthRepository(UserManager<AuthUser> userManager, 
        SignInManager<AuthUser> signInManager, 
        ApplicationContext applicationContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationContext = applicationContext;
    }
    public Task<User?> GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> UserValid(User reqUser)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> CreateUser(User user)
    {
        // Keep all the exception handling for bad user credentials
        {
            // All error descriptions listed in single string
            // var errorString = string.Join("\n", result.Errors.Select(e => e.Description));
            //
            // var badValues = result.Errors.Any(x => x.Code is 
            //     nameof(IdentityErrorDescriber.PasswordRequiresDigit) or
            //     nameof(IdentityErrorDescriber.PasswordRequiresLower) or
            //     nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric) or 
            //     nameof(IdentityErrorDescriber.PasswordTooShort) or
            //     nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars) or
            //     nameof(IdentityErrorDescriber.PasswordRequiresUpper) or
            //     nameof(IdentityErrorDescriber.InvalidUserName));
            //
            // if (badValues) throw new BadValuesException(errorString);
            //
            // var duplicateValues = result.Errors.Any(x => x.Code is 
            //     nameof(IdentityErrorDescriber.DuplicateUserName) or
            //     nameof(IdentityErrorDescriber.DuplicateEmail));
            //
            // if (duplicateValues) throw new EntityAlreadyExistsException(errorString);
        }

        throw new NotImplementedException();
    }
}