using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserById(Guid id, bool includeNavProperties = true)
    {
        User? user;
        if (includeNavProperties)
        {
            user = await _context.Users
                .Include(x => x.Friends)
                .Include(x => x.SentFriendRequests)
                .Include(x => x.ReceivedFriendRequests)
                .SingleOrDefaultAsync(u => u.Id == id);
        }
        else
        {
            user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);
        }
        
        return user;
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}