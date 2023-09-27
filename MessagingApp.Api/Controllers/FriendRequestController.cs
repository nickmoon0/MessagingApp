﻿using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.FriendRequests.Commands.AcceptFriendRequest;
using MessagingApp.Application.FriendRequests.Commands.CreateFriendRequest;
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