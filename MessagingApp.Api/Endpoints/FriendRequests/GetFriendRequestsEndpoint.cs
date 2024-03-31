using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.GetFriendRequests;
using MessagingApp.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.FriendRequests;

public class GetFriendRequestsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Retrieves all friend requests for the requesting user")
        .WithDescription("Use query parameters to retrieve list of friend requests that a user sent. Returns two lists (one for outgoing and one for incoming requests)")
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<GetFriendRequestsEndpointResponse>();
    
    private static async Task<IResult> Handle(
        [FromQuery] bool? sent,
        [FromQuery] bool? received,
        [FromQuery] FriendRequestStatus? status,
        [FromServices] IHandler<GetFriendRequestsQuery, GetFriendRequestsResponse> handler,
        HttpContext context)
    {
        var userId = (Guid?)context.Items[Helpers.UserIdKey];
        if (userId == null) return Results.Unauthorized();
        
        if (sent == null && received == null) 
            return Results.Ok(new GetFriendRequestsResponse { ReceivedFriendRequests = [], SentFriendRequests = []});

        var query = new GetFriendRequestsQuery
        {
            UserId = (Guid)userId,
            GetReceivedRequests = received ?? false,
            GetSentRequests = sent ?? false,
            Status = status
        };

        var handlerResult = await handler.Handle(query);
        if (!handlerResult.IsOk) return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));

        var handlerResponse = handlerResult.Value;
        var response = new GetFriendRequestsEndpointResponse(
            handlerResponse.SentFriendRequests,
            handlerResponse.ReceivedFriendRequests);

        return Results.Ok(response);
    }
}

public record GetFriendRequestsEndpointResponse(ICollection<FriendRequestResponse> SentRequests, ICollection<FriendRequestResponse> ReceivedRequests);