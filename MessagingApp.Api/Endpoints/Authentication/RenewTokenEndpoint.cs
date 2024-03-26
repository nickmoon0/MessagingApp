using MessagingApp.Api.Common;
using MessagingApp.Application.Common;
using MessagingApp.Application.Features.RenewTokens;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.Authentication;

public class RenewTokenEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/renewToken", Handle)
        .WithSummary("Returns new tokens")
        .WithDescription(
            "Returns a new access token in the response body and a new refresh token in the header as a HTTP Only cookie")
        .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<RenewTokenEndpointResponse>();

    public static async Task<IResult> Handle(
        [FromServices] IHandler<RenewTokenCommand, RenewTokenResponse> handler,
        HttpContext context)
    {
        var refreshToken = context.Request.Cookies["RefreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Results.Json(
                new ErrorResponse("Refresh token not present in header"), 
                statusCode: StatusCodes.Status401Unauthorized);

        var command = new RenewTokenCommand { RefreshToken = refreshToken };

        var handlerResult = await handler.Handle(command);
        if (!handlerResult.IsOk) return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));
        
        var tokens = handlerResult.Value.Tokens;

        Helpers.AddRefreshTokenCookie(context, tokens.NewRefreshToken!.Token!);
        return Results.Ok(new RenewTokenEndpointResponse(tokens.NewAccessToken!));
    }
}

public record RenewTokenEndpointResponse(string AccessToken);