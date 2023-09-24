using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data.Contexts;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
}