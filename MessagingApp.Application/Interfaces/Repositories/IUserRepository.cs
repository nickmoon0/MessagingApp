using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Interfaces.Repositories;

public interface IUserRepository
{
    public User GetUserById(Guid id);
    public User GetUserByUsername(string username);
    
    // TODO: Figure out what parameters we need
    public Guid CreateUser();
}