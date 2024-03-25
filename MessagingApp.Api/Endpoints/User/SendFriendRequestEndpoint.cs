using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.SendFriendRequest;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.User;

public class SendFriendRequestEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/{receivingUserId:guid}/add", Handle)
        .WithSummary("Sends a friend request")
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<SendFriendRequestResponse>(StatusCodes.Status201Created);

    private static async Task<IResult> Handle(
        [FromRoute] Guid receivingUserId,
        [FromServices] ITokenService tokenService,
        [FromServices] IHandler<SendFriendRequestCommand, SendFriendRequestResponse> handler,
        HttpContext context)
    {
        var token = Helpers.GetAccessToken(context);
        if (!token.IsOk) return Results.StatusCode(StatusCodes.Status500InternalServerError);
        var sendingUserId = tokenService.ExtractUserIdFromAccessToken(token.Value);
        
        var command = new SendFriendRequestCommand
        {
            SendingUserId = sendingUserId,
            ReceivingUserId = receivingUserId
        };

        var result = await handler.Handle(command);
        if (!result.IsOk) return Results.BadRequest(new ErrorResponse(result.Error.Message));
        var friendRequest = result.Value;
        
        var response = new SendFriendRequestEndpointResponse(
            friendRequest.Id, friendRequest.SendingUserId, friendRequest.ReceivingUserId);

        return Results.Created($"/friendRequest/{friendRequest.Id}", response);
    }
}

public record SendFriendRequestEndpointResponse(Guid FriendRequestId, Guid SendingUserId, Guid ReceivingUserId);