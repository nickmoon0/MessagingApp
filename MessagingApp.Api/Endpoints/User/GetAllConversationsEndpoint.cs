using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.GetAllConversations;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.User;

public class GetAllConversationsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/conversations", Handle)
        .WithSummary("Retrieves all a users conversations")
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<GetAllConversationsEndpointResponse>();

    private static async Task<IResult> Handle(
        [FromServices] IHandler<GetAllConversationsQuery, GetAllConversationsResponse> handler,
        HttpContext context)
    {
        var userId = (Guid?)context.Items[Helpers.UserIdKey];
        if (userId == null) return Results.Unauthorized();

        var query = new GetAllConversationsQuery { UserId = (Guid)userId };
        var handlerResult = await handler.Handle(query);

        return handlerResult.IsOk
            ? Results.Ok(new GetAllConversationsEndpointResponse(handlerResult.Value.Conversations))
            : Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));
    }
}

public record GetAllConversationsEndpointResponse(IEnumerable<ConversationSummaryResponse> Conversations);