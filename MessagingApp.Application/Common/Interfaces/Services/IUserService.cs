using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Services;

public interface IUserService
{
    public Task<User?> GetUserByUsername(string username, bool includeNavProperties = true);
    public Task<User?> GetUserById(Guid id, bool includeNavProperties = true);
    public Task UpdateUser(User user);
    public Task<FriendRequest?> GetFriendRequestById(Guid id);
}