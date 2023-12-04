using Microsoft.AspNetCore.Identity;

namespace MessagingApp.Infrastructure.Data.Models;

/// <summary>
/// AuthUser allows IdentityUsers to be created with Guid primary key
/// </summary>
public class AuthUser : IdentityUser<Guid>
{
    
}