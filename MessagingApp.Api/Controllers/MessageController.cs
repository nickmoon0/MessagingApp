using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Users.Commands.SendMessage;
using MessagingApp.Application.Users.Queries.GetMessageById;
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
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessage(SendMessageRequest sendMessageRequest)
    {
        var command = new SendMessageCommand(sendMessageRequest, UserId);
        var result = await _mediator.Send(command);
        return result.ToCreated($"/message", x => x);
    }

    [HttpGet("{messageId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetMessageById([FromRoute] Guid messageId)
    {
        var query = new GetMessageByIdQuery(messageId, UserId);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
}