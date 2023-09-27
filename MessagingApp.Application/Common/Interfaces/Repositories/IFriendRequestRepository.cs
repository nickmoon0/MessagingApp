using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IFriendRequestRepository
{
    public Task<FriendRequest?> GetFriendRequestById(Guid id);
}