using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Features.GetUser;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.User;

public class GetUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{userId:guid}", Handle)
        .WithSummary("Returns a users details to caller")
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<GetUserEndpointResponse>();

    public static async Task<IResult> Handle(
        [FromRoute] Guid userId,
        [FromServices] IHandler<GetUserQuery, GetUserResponse> handler)
    {
        var query = new GetUserQuery { UserId = userId };
        var handlerResult = await handler.Handle(query);
        if (!handlerResult.IsOk) return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));

        var handlerResponse = handlerResult.Value;
        return Results.Ok(new GetUserEndpointResponse(handlerResponse));
    }
}

public record GetUserEndpointResponse(GetUserResponse User);