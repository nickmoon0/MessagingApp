using MessagingApp.Application.Common.DTOs;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<UserDto?> GetUserById(Guid id);
    public Task<UserDto?> GetUserByUsername(string username);
    public Task<bool> UserValid(User user);
    
    public Task<UserDto?> CreateUser(User user);
}