using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace MessagingApp.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            var uidString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                            ?? throw new InvalidOperationException();
            return Guid.Parse(uidString);
        }
    }

    protected string GetClaimValue(string claimType) => 
        User.FindFirst(claimType)?.Value ?? throw new InvalidOperationException();
}
