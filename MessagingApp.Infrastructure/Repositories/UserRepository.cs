using MessagingApp.Application.Interfaces.Repositories;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Contexts;

namespace MessagingApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthContext _authContext;
    public UserRepository(AuthContext authContext)
    {
        _authContext = authContext;
    }
    public User? GetUserById(Guid id)
    {
        var user = _authContext.Users.SingleOrDefault(x => x.Id == id);
        return user;
    }

    public User? GetUserByUsername(string username)
    {
        var user = _authContext.Users.SingleOrDefault(x => x.Username == username);
        return user;
    }

    public Guid CreateUser(User user)
    {
        _authContext.Users.Add(user);
        _authContext.SaveChanges();

        return user.Id;
    }
}