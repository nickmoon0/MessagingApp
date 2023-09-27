using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly ApplicationContext _context;

    public FriendRequestRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<FriendRequest?> GetFriendRequestById(Guid id)
    {
        var friendRequest = await _context.FriendRequests.SingleOrDefaultAsync(x => x.Id == id);
        return friendRequest;
    }
}