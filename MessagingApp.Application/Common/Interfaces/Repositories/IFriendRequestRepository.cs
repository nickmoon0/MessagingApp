using MessagingApp.Application.Common.DTOs;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IFriendRequestRepository
{
    public Task<List<FriendRequestDto>> GetSentFriendRequests(UserDto user);
    public Task<List<FriendRequestDto>> GetReceivedFriendRequests(UserDto user);
    public Task SetFriendRequestStatus(FriendRequestDto friendRequest);
}