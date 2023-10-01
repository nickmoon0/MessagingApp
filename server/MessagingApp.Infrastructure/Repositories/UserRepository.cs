using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsername(string username, bool includeNavProperties = true)
    {
        User? user;
        
        if (includeNavProperties)
        {
            user = await _context.Users
                .Include(x => x.Friends)
                .Include(x => x.SentFriendRequests)
                .Include(x => x.ReceivedFriendRequests)
                .SingleOrDefaultAsync(u => u.Username == username);
        }
        else
        {
            user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == username);
        }
        
        return user;
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
                .Include(x => x.SentMessages)
                .Include(x => x.ReceivedMessages)
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

    public async Task<IEnumerable<FriendRequest>> GetUsersFriendRequests(Guid userId, FriendRequestStatus status)
    {
        var friendRequests = _context.FriendRequests.Where(x =>
            (x.FromUserId == userId && x.Status == status) ||
            (x.ToUserId == userId && x.Status == status));

        return friendRequests;
    }
    
    public async Task<FriendRequest?> GetFriendRequestById(Guid id)
    {
        var friendRequest = await _context.FriendRequests.SingleOrDefaultAsync(x => x.Id == id);
        return friendRequest;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestingUser">The user requesting the conversation</param>
    /// <param name="conversationUser">The other user who is part of the conversation</param>
    /// <returns></returns>
    public async Task<IEnumerable<Message>> GetConversation(Guid requestingUser, Guid conversationUser)
    {
        var sentMessages = _context.Messages.Where(msg =>
            (msg.SendingUserId == requestingUser && msg.ReceivingUserId == conversationUser) ||
            (msg.SendingUserId == conversationUser && msg.ReceivingUserId == requestingUser));
        
        return sentMessages;
    }

    public async Task<Message?> GetMessageById(Guid requestingUser, Guid id)
    {
        var message = await _context.Messages.SingleOrDefaultAsync(msg => 
            (msg.SendingUserId == requestingUser && msg.Id == id) ||
            (msg.ReceivingUserId == requestingUser && msg.Id == id));

        return message;
    }
}