using MessagingApp.Application.Commands;
using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
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
    public IActionResult Register(CreateUserDto createUserDto)
    {
        var command = new CreateUserCommand(createUserDto);
        var result = _mediator.Send(command);
        return Created("", result);
    }
}