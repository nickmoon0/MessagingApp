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
    public User GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public User GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Guid CreateUser(User user)
    {
        _authContext.Users.Add(user);
        _authContext.SaveChanges();

        return user.Id;
    }
}