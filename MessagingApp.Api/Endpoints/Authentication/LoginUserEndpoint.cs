﻿using FluentValidation;
using MessagingApp.Api.Common;
using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common;
using MessagingApp.Application.Features.LoginUser;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.Authentication;

public class LoginUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/login", Handle)
        .WithSummary("Authenticates user")
        .WithDescription("Checks users credentials are correct and issues tokens if they are")
        .WithRequestValidation<LoginUserEndpointRequest>()
        .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized)
        .Produces<LoginUserEndpointResponse>();

    private static async Task<IResult> Handle(
        [FromBody] LoginUserEndpointRequest request,
        [FromServices] IHandler<LoginUserCommand, LoginUserResponse> handler,
        HttpContext context)
    {
        var command = new LoginUserCommand
        {
            Username = request.Username,
            Password = request.Password
        };

        var result = await handler.Handle(command);
        if (!result.IsOk) return Results.Json(
            new ErrorResponse(result.Error.Message),
            statusCode: StatusCodes.Status401Unauthorized);

        var tokens = result.Value;
        var refreshToken = tokens.Tokens.NewRefreshToken;
        
        Helpers.AddRefreshTokenCookie(context, refreshToken!.Token!);
        return Results.Ok(new LoginUserEndpointResponse(tokens.Tokens.NewAccessToken!));
    } 
    
}

public record LoginUserEndpointRequest(string Username, string Password);
public record LoginUserEndpointResponse(string AccessToken);

public class LoginUserRequestValidator : AbstractValidator<LoginUserEndpointRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}