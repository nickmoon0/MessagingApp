using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.RespondToFriendRequest;
using MessagingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.FriendRequests;

public class RespondToFriendRequestEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/{friendRequestId:guid}", Handle)
        .WithSummary("Accept or reject a friend request")
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<RespondToFriendRequestEndpointResponse>();

    public static async Task<IResult> Handle(
        [FromRoute] Guid friendRequestId,
        [FromBody] RespondToFriendRequestEndpointRequest request,
        [FromServices] ITokenService tokenService,
        [FromServices] IHandler<RespondToFriendRequestCommand, RespondToFriendRequestResponse> handler,
        HttpContext context)
    {
        var token = Helpers.GetAccessToken(context);
        if (!token.IsOk) return Results.StatusCode(StatusCodes.Status500InternalServerError);
        var respondingUserId = tokenService.ExtractUserIdFromAccessToken(token.Value);

        var command = new RespondToFriendRequestCommand
        {
            FriendRequestId = friendRequestId,
            RespondingUserId = respondingUserId,
            Response = request.Status
        };

        var responseResult = await handler.Handle(command);
        if (!responseResult.IsOk) return Results.BadRequest(new ErrorResponse(responseResult.Error.Message));

        var friendRequestResponse = responseResult.Value;
        var response = new RespondToFriendRequestEndpointResponse(friendRequestResponse.FriendRequestId, friendRequestResponse.NewStatus);
        
        return Results.Ok(response);
    }
}

public record RespondToFriendRequestEndpointRequest(FriendRequestStatus Status);
public record RespondToFriendRequestEndpointResponse(Guid FriendRequestId, FriendRequestStatus NewStatus);