using FluentValidation;
using MessagingApp.Api.Common;
using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common;
using MessagingApp.Application.Features.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.Authentication;

public abstract class RegisterUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/register", Handle)
        .WithSummary("Registers a user")
        .WithDescription("Registers a user with the given username and password. Bio is optional.")
        .WithRequestValidation<RegisterUserEndpointRequest>()
        .Produces<RegisterUserEndpointResponse>(StatusCodes.Status201Created);
    
    private static async Task<IResult> Handle(
        [FromBody] RegisterUserEndpointRequest request,
        [FromServices] IHandler<RegisterUserCommand, RegisterUserResponse> handler)
    {
        var command = new RegisterUserCommand
        {
            Username = request.Username,
            Password = request.Password,
            Bio = request.Bio
        };

        var result = await handler.Handle(command);
        if (!result.IsOk) return Results.BadRequest(new ErrorResponse(result.Error.Message));
        
        var registration = result.Value;
        var response = new RegisterUserEndpointResponse(
            registration.Id, registration.Username, registration.Tokens.NewAccessToken!, registration.Bio);
        
        return Results.Created($"/user/{result.Value.Id}", response);
    }
}
public record RegisterUserEndpointRequest(string Username, string Password, string? Bio = null);
public record RegisterUserEndpointResponse(Guid Id, string Username, string AccessToken, string? Bio = null);

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserEndpointRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}