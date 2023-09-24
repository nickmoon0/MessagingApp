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

    public async Task<User?> GetUserById(Guid id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == id);
        return user;
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}