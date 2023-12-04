using Microsoft.AspNetCore.Identity;

namespace MessagingApp.Infrastructure.Data.Models;

/// <summary>
/// AuthRole allows IdentityRoles to be created with Guid primary key
/// </summary>
public class AuthRole : IdentityRole<Guid>
{
    
}