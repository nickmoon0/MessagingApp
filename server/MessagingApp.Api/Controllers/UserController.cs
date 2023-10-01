using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.CreateFriendRequest;
using MessagingApp.Application.Users.Commands.SendMessage;
using MessagingApp.Application.Users.Queries.RetrieveConversation;
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
    
    [HttpPost("{toUserId:guid}/add")]
    [Authorize]
    public async Task<IActionResult> SendFriendRequest([FromRoute] Guid toUserId)
    {
        var command = new CreateFriendRequestCommand(toUserId, UserId);
        var result = await _mediator.Send(command);
        return result.ToOk();
    }
    
    [HttpGet("{conversationUserId:guid}/messages")]
    [Authorize]
    public async Task<IActionResult> GetConversation([FromRoute] Guid conversationUserId)
    {
        var query = new RetrieveConversationQuery(UserId, conversationUserId);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
    
    [HttpPost("{receivingUserId:guid}/message")]
    [Authorize]
    public async Task<IActionResult> SendMessage(
        [FromBody] SendMessageRequest sendMessageRequest,
        [FromRoute] Guid receivingUserId)
    {
        var command = new SendMessageCommand(sendMessageRequest, receivingUserId, UserId);
        var result = await _mediator.Send(command);
        return result.ToCreated($"/message", x => x);
    }
}