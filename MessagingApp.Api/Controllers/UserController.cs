using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Queries;
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
    public IActionResult GetUser(RetrieveUserDto retrieveUserDto)
    {
        try
        {
            var query = new RetrieveUserQuery(retrieveUserDto);
            var result = _mediator.Send(query);
            return result is null ? NotFound() : Ok(result);
        }
        catch (NotEnoughDetailsException)
        {
            return BadRequest();
        }
    }
}