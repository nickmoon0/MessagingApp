using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;
    
    public UserRepository(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public Task<User?> GetUserById(Guid id)
    {
        throw new NotImplementedException();
        // var user = _authContext.Users.SingleOrDefault(x => x.Id == id);
        // return user;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var retrievedUser = await _userManager.FindByNameAsync(username);
        if (retrievedUser == null) return null;
        
        var user = new User()
        {
            Id = retrievedUser.Id,
            Username = retrievedUser.UserName
        };
        
        return user;
    }

    public async Task<bool> UserValid(User reqUser)
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

    public Task<Guid> CreateUser(User user)
    {
        throw new NotImplementedException();
        
        // try
        // {
        //     _authContext.Users.Add(user);
        //     _authContext.SaveChanges();
        //
        //     return user.Id;
        // }
        // catch (DbUpdateException)
        // {
        //     throw new EntityAlreadyExistsException($"User with username {user.Username} already exists");
        // }
    }
}