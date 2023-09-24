using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;
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
        if (user.Password == null) throw new InvalidOperationException();

        // Create user in auth database
        var authUser = new AuthUser { UserName = user.Username };
        var result = await _userManager.CreateAsync(authUser, user.Password);

        // Create in app database if exists
        if (result.Succeeded)
        {
            _applicationContext.Users.Add(user);
            await _applicationContext.SaveChangesAsync();
        }
        else
        {
            // All error descriptions listed in single string
            var errorString = string.Join("\n", result.Errors.Select(e => e.Description));
            
            var badValues = result.Errors.Any(x => x.Code is 
                nameof(IdentityErrorDescriber.PasswordRequiresDigit) or
                nameof(IdentityErrorDescriber.PasswordRequiresLower) or
                nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric) or 
                nameof(IdentityErrorDescriber.PasswordTooShort) or
                nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars) or
                nameof(IdentityErrorDescriber.PasswordRequiresUpper) or
                nameof(IdentityErrorDescriber.InvalidUserName));
            
            if (badValues) throw new BadValuesException(errorString);
            
            var duplicateValues = result.Errors.Any(x => x.Code is 
                nameof(IdentityErrorDescriber.DuplicateUserName) or
                nameof(IdentityErrorDescriber.DuplicateEmail));
            
            if (duplicateValues) throw new EntityAlreadyExistsException(errorString);
        }

        // Null out password so it doesnt get passed around application
        user.Password = null;
        
        return user;
    }
}