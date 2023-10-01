using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.AcceptFriendRequest;
using MessagingApp.Application.Users.Commands.CreateFriendRequest;
using MessagingApp.Application.Users.Queries.RetrievePendingFriendRequests;
using MessagingApp.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FriendRequestController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<FriendRequestController> _logger;
    
    public FriendRequestController(IMediator mediator, ILogger<FriendRequestController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> RetrievePendingFriendRequests()
    {
        var query = new RetrievePendingFriendRequestsQuery(UserId);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendFriendRequest(CreateFriendRequestRequest createFriendRequest)
    {
        var command = new CreateFriendRequestCommand(createFriendRequest, UserId);
        var result = await _mediator.Send(command);
        return result.ToOk();
    }

    [HttpPut("accept/{friendRequestId:guid}")]
    [Authorize]
    public async Task<IActionResult> AcceptFriendRequest([FromRoute] Guid friendRequestId)
    {
        var request = new AcceptFriendRequestRequest { FriendRequestId = friendRequestId };
        var command = new AcceptFriendRequestCommand(request, UserId);
        var result = await _mediator.Send(command);
        return result.ToOk();
    }
}