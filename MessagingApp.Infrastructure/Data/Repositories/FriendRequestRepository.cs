using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    public Task<List<FriendRequest>> GetSentFriendRequests(User user)
    {
        throw new NotImplementedException();
    }

    public Task<List<FriendRequest>> GetReceivedFriendRequests(User user)
    {
        throw new NotImplementedException();
    }

    public Task SetFriendRequestStatus(FriendRequest friendRequest)
    {
        throw new NotImplementedException();
    }
}