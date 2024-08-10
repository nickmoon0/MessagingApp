using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.GetConversation;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.Conversation;

public class GetConversationEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{conversationId:guid}", Handle)
        .WithSummary("Retrieves detailed information about a conversation")
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<GetConversationEndpointResponse>();

    private static async Task<IResult> Handle(
        [FromRoute] Guid conversationId,
        [FromQuery] int? numberOfMessages,
        [FromServices] IHandler<GetConversationQuery, GetConversationResponse> handler,
        HttpContext context)
    {
        var userId = (Guid?)context.Items[Helpers.UserIdKey];
        if (userId == null) return Results.Unauthorized();

        var query = new GetConversationQuery
        {
            UserId = (Guid)userId,
            ConversationId = conversationId,
            MessagesToRetrieve = numberOfMessages
        };

        var handlerResult = await handler.Handle(query);
        if (!handlerResult.IsOk) return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));

        var response = handlerResult.Value;
        return Results.Ok(new GetConversationEndpointResponse(response));
    }
}

public record GetConversationEndpointResponse(GetConversationResponse Conversation);