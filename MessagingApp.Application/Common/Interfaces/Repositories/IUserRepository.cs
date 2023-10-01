using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByUsername(string username, bool includeNavProperties = true);
    public Task<User?> GetUserById(Guid id, bool includeNavProperties = true);
    public Task UpdateUser(User user);
    public Task<FriendRequest?> GetFriendRequestById(Guid id);
    public Task<IEnumerable<Message>> GetConversation(Guid sendingUser, Guid receivingUser);
    public Task<Message?> GetMessageById(Guid requestingUser, Guid id);
}