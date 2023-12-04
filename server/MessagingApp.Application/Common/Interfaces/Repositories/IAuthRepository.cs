using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

/// <summary>
/// Repository for handling authentication. Application operations are not defined here
/// </summary>
public interface IAuthRepository
{
    public Task<User?> GetUserById(Guid id);
    public Task<User?> GetUserByUsername(string username);
    public Task<bool> UserValid(User user);
    
    public Task<User?> CreateUser(User user);
}