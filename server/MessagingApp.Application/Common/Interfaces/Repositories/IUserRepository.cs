using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

/// <summary>
/// Repository for handling all application user operations. Auth operations are not defined here.
/// </summary>
public interface IUserRepository
{
    public Task<User?> GetUserByUsername(string username, bool includeNavProperties = true);
    public Task<User?> GetUserById(Guid id, bool includeNavProperties = true);
    public Task UpdateUser(User user);
    public Task<IEnumerable<FriendRequest>> GetUsersFriendRequests(Guid userId, FriendRequestStatus status);
    public Task<FriendRequest?> GetFriendRequestById(Guid id);
    public Task<IEnumerable<Message>> GetConversation(Guid sendingUser, Guid receivingUser);
    public Task<Message?> GetMessageById(Guid requestingUser, Guid id);
    public Task<List<User>> GetUsersFriends(Guid requestingUser);
}