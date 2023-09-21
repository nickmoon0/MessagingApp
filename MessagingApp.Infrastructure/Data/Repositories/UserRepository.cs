using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IFriendRequestRepository
{
    public Task<List<FriendRequestDto>> GetSentFriendRequests(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task<List<FriendRequestDto>> GetReceivedFriendRequests(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task SetFriendRequestStatus(FriendRequestDto friendRequest)
    {
        throw new NotImplementedException();
    }
}