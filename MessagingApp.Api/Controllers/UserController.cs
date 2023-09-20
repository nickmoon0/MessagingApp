using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Queries.RetrieveUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
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
    public async Task<IActionResult> GetUser(RetrieveUserDto retrieveUserDto)
    {
        var query = new RetrieveUserQuery(retrieveUserDto);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
}