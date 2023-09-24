using MessagingApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Contexts;

public class AuthContext : IdentityDbContext<AuthUser, AuthRole, Guid>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

}