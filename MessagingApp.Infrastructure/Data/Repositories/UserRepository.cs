using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IFriendRequestRepository
{
    private ApplicationContext _context;
    private UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<List<FriendRequestDto>> GetSentFriendRequests(UserDto user)
    {
        var friendReqList = new List<FriendRequestDto>();
        
        // Get pending status
        var pendingStatus = await _context.RequestStatuses
            .SingleOrDefaultAsync(x => x.Id == RequestStatuses.Pending);
        
        // Sent request = UserId with pending status
        
        
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