using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.GetFriends;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.User;

public class GetFriendsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/friends", Handle)
        .WithSummary("Returns all of the calling users friends")
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<GetFriendsEndpointResponse>();

    public static async Task<IResult> Handle(
        [FromServices] ITokenService tokenService,
        [FromServices] IHandler<GetFriendsQuery, GetFriendsResponse> handler,
        HttpContext context)
    {
        var token = Helpers.GetAccessToken(context);
        if (!token.IsOk) return Results.StatusCode(StatusCodes.Status401Unauthorized);
        var userId = tokenService.ExtractUserIdFromAccessToken(token.Value);

        var query = new GetFriendsQuery { UserId = userId };
        var handlerResult = await handler.Handle(query);

        return handlerResult.IsOk
            ? Results.Ok(new GetFriendsEndpointResponse(handlerResult.Value.Friends))
            : Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));
    }
}

public record GetFriendsEndpointResponse(IEnumerable<UserSummaryResponse> Friends);