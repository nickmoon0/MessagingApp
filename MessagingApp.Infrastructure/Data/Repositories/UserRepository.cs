using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Infrastructure.Data.Contexts;
using MessagingApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IFriendRequestRepository, IUserRepository
{
    private readonly ApplicationContext _context;
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<List<FriendRequestDto>> GetSentFriendRequests(UserDto user)
    {
        throw new NotImplementedException();
    }

    public async Task<List<FriendRequestDto>> GetReceivedFriendRequests(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task SetFriendRequestStatus(FriendRequestDto friendRequest)
    {
        throw new NotImplementedException();
    }
}