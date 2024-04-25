using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Features.GetUserByName;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.User;

public class GetUserByUsernameEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/username/{username}", Handle)
        .WithSummary("Returns user details by username")
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<GetUserByUsernameEndpointResponse>(StatusCodes.Status200OK);

    public static async Task<IResult> Handle(
        [FromRoute] string username,
        [FromServices] IHandler<GetUserByNameQuery, GetUserByNameResponse> handler)
    {
        var query = new GetUserByNameQuery { Username = username };
        var handlerResult = await handler.Handle(query);
        if (!handlerResult.IsOk)
            return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));

        var handlerResponse = handlerResult.Value;
        return Results.Ok(new GetUserByUsernameEndpointResponse(handlerResponse));
    }
}

public record GetUserByUsernameEndpointResponse(GetUserByNameResponse User);