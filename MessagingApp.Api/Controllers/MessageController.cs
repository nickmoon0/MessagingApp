using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.SendMessage;
using MessagingApp.Application.Users.Queries.RetrieveMessageById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : BaseController
{
    private readonly IMediator _mediator;
    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("{receivingUserId:guid}")]
    [Authorize]
    public async Task<IActionResult> SendMessage(
        [FromBody] SendMessageRequest sendMessageRequest,
        [FromRoute] Guid receivingUserId)
    {
        var command = new SendMessageCommand(sendMessageRequest, receivingUserId, UserId);
        var result = await _mediator.Send(command);
        return result.ToCreated($"/message", x => x);
    }

    [HttpGet("{messageId:guid}")]
    [Authorize]
    public async Task<IActionResult> RetrieveMessageById([FromRoute] Guid messageId)
    {
        var query = new RetrieveMessageByIdQuery(messageId, UserId);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
}