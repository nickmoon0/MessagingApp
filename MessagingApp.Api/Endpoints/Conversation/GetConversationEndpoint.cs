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
        [FromServices] ITokenService tokenService,
        [FromServices] IHandler<GetConversationQuery, GetConversationResponse> handler,
        HttpContext context)
    {
        var token = Helpers.GetAccessToken(context);
        if (!token.IsOk) return Results.StatusCode(StatusCodes.Status401Unauthorized);
        var userId = tokenService.ExtractUserIdFromAccessToken(token.Value);

        var query = new GetConversationQuery
        {
            UserId = userId,
            ConversationId = conversationId
        };

        var handlerResult = await handler.Handle(query);
        if (!handlerResult.IsOk) return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));

        var response = handlerResult.Value;
        return Results.Ok(new GetConversationEndpointResponse(response));
    }
}

public record GetConversationEndpointResponse(GetConversationResponse Conversation);