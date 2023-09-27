using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.CreateUser;
using MessagingApp.Application.Users.Queries.AuthenticateUser;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpPost]
    [Route(nameof(Register))]
    public async Task<IActionResult> Register(CreateUserRequest createUserRequest)
    {
        var command = new CreateUserCommand(createUserRequest);
        var result = await _mediator.Send(command);
        return result.ToCreated("/user");
    }

    [HttpPost]
    [Route(nameof(Authenticate))]
    public async Task<IActionResult> Authenticate(AuthenticateUserRequest authenticateUserRequest)
    {
        var query = new AuthenticateUserQuery(authenticateUserRequest);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
}