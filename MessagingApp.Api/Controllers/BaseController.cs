using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            var uidString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new InvalidOperationException();
            return Guid.Parse(uidString);
        }
    }

    protected string GetClaimValue(string claimType) => 
        User.FindFirst(claimType)?.Value ?? throw new InvalidOperationException();
}
