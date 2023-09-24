using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data.Contexts;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IFriendRequestRepository, IUserRepository
{
    private readonly ApplicationContext _context;
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
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