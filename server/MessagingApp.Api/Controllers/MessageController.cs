using MediatR;
using MessagingApp.Api.Extensions;
using MessagingApp.Application.MessageFeatures.RetrieveMessageById;
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

    [HttpGet("{messageId:guid}")]
    [Authorize]
    public async Task<IActionResult> RetrieveMessageById([FromRoute] Guid messageId)
    {
        var query = new RetrieveMessageByIdQuery(messageId, UserId);
        var result = await _mediator.Send(query);
        return result.ToOk();
    }
}