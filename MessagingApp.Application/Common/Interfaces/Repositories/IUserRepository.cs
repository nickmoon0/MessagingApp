using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserById(Guid id);
    public Task<User?> GetUserByUsername(string username);
    public Task<bool> UserValid(User user);
    
    public Task<Guid> CreateUser(User user);
}