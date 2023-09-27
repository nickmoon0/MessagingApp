using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.FriendRequests.Commands.CreateFriendRequest;
using MessagingApp.Application.Users.Queries.RetrieveUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;
    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser([FromQuery] Guid? uid, [FromQuery] string? username)
    {
        var request = new RetrieveUserRequest
        {
            Id = uid,
            Username = username
        };
        var query = new RetrieveUserQuery(request);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
}