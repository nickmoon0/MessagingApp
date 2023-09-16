using MessagingApp.Application.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public User GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public User GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Guid CreateUser()
    {
        throw new NotImplementedException();
    }
}