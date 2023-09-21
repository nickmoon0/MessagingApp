using MessagingApp.Application.Common.DTOs;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<UserDto?> GetUserById(Guid id);
    public Task<UserDto?> GetUserByUsername(string username);
    public Task<bool> UserValid(UserDto user);
    
    public Task<UserDto?> CreateUser(UserDto user);
}