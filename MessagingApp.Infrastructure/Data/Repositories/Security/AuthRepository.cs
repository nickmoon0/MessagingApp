using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Infrastructure.Data.Models.Security;
using Microsoft.AspNetCore.Identity;

namespace MessagingApp.Infrastructure.Data.Repositories.Security;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;
    
    public AuthRepository(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public Task<UserDto?> GetUserById(Guid id)
    {
        throw new NotImplementedException();
        // var user = _authContext.Users.SingleOrDefault(x => x.Id == id);
        // return user;
    }

    public async Task<UserDto?> GetUserByUsername(string username)
    {
        var retrievedUser = await _userManager.FindByNameAsync(username);
        if (retrievedUser == null) return null;
        
        var user = new UserDto
        {
            Id = retrievedUser.Id,
            Username = retrievedUser.UserName
        };
        
        return user;
    }

    public async Task<bool> UserValid(UserDto reqUser)
    {
        if (reqUser.Username == null || reqUser.Password == null)
            return false;
        
        var user = await _userManager.FindByNameAsync(reqUser.Username);
        if (user == null)
            return false;

        // Check the password
        var result = await _signInManager.CheckPasswordSignInAsync(user, reqUser.Password, false);
        return result.Succeeded;
    }

    public async Task<UserDto?> CreateUser(UserDto user)
    {
        var authUser = new AuthUser { UserName = user.Username };
        var result = await _userManager.CreateAsync(authUser, user.Password!);

        if (!result.Succeeded)
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
        
        var createdUser = new UserDto
        {
            Id = authUser.Id,
            Username = authUser.UserName,
        };
        
        return result.Succeeded ? createdUser : null;
    }
}