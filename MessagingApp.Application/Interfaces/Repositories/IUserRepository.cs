using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Interfaces.Repositories;

public interface IUserRepository
{
    public User GetUserById(Guid id);
    public User GetUserByUsername(string username);
    
    public Guid CreateUser(User user);
}