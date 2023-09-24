using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                               ?? throw new AuthenticationException();
    protected string GetClaimValue(string claimType) => User.FindFirst(claimType)?.Value 
                                                        ?? throw new AuthenticationException();
}