using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserById(Guid id);
    public Task UpdateUser(User user);
}