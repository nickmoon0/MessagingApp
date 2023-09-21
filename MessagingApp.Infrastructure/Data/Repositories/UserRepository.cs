using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IFriendRequestRepository
{
    private readonly ApplicationContext _context;
    private UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<List<FriendRequestDto>> GetSentFriendRequests(UserDto user)
    {
        var friendReqList = new List<FriendRequestDto>();
        
        // Get pending status
        var pendingStatus = await _context.RequestStatuses
            .SingleAsync(x => x.Id == RequestStatuses.Pending);
        
        // Sent request = UserId with pending status
        var friendRequests = _context.UserFriends
            .Where(x => x.UserId == user.Id && x.Status.Id == pendingStatus.Id);

        foreach (var userFriend in friendRequests)
        {
            var requestDto = new FriendRequestDto()
            {
                ToUser = userFriend.FriendId,
                FromUser = userFriend.UserId,
                Status = FriendRequestDtoStatus.Pending
            };         
            friendReqList.Add(requestDto);
        }

        return friendReqList;
    }

    public async Task<List<FriendRequestDto>> GetReceivedFriendRequests(UserDto user)
    {
        var friendReqList = new List<FriendRequestDto>();
        
        // Get pending status
        var pendingStatus = await _context.RequestStatuses
            .SingleAsync(x => x.Id == RequestStatuses.Pending);
        
        // Sent request = UserId with pending status
        var friendRequests = _context.UserFriends
            .Where(x => x.FriendId == user.Id && x.Status.Id == pendingStatus.Id);
        
        foreach (var userFriend in friendRequests)
        {
            var requestDto = new FriendRequestDto()
            {
                ToUser = userFriend.UserId,
                FromUser = userFriend.FriendId,
                Status = FriendRequestDtoStatus.Pending
            };         
            friendReqList.Add(requestDto);
        }

        return friendReqList;
    }

    public Task SetFriendRequestStatus(FriendRequestDto friendRequest)
    {
        throw new NotImplementedException();
    }
}