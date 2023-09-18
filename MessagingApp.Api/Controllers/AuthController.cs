﻿using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.CreateUser;
using MessagingApp.Application.Users.Queries.AuthenticateUser;
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
        return result.ToCreated("", guid => guid);
    }

    [HttpPost]
    [Route(nameof(Authenticate))]
    public IActionResult Authenticate(AuthenticateUserDto authenticateUserDto)
    {
        var query = new AuthenticateUserQuery(authenticateUserDto);
        var result = _mediator.Send(query);
        return result.ToOk(x => x);
    }
}