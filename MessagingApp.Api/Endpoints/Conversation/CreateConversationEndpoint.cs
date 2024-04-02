using FluentValidation;
using MessagingApp.Api.Common;
using MessagingApp.Api.Extensions;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Application.Features.CreateConversation;
using MessagingApp.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Api.Endpoints.Conversation;

public class CreateConversationEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/create", Handle)
        .WithSummary("Creates a new conversation between users")
        .WithDescription("Can create ")
        .WithRequestValidation<CreateConversationEndpointRequest>()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<CreateConversationEndpointResponse>(StatusCodes.Status201Created);

    private static async Task<IResult> Handle(
        [FromBody] CreateConversationEndpointRequest request,
        [FromServices] IHandler<CreateConversationCommand, CreateConversationResponse> handler,
        HttpContext context)
    {
        var userId = (Guid?)context.Items[Helpers.UserIdKey];
        if (userId == null) return Results.Unauthorized();

        var command = new CreateConversationCommand
        {
            RequestingUserId = (Guid)userId,
            Type = request.Type,
            ParticipantIds = request.Participants
        };
        var handlerResult = await handler.Handle(command);
        if (!handlerResult.IsOk) return Results.BadRequest(new ErrorResponse(handlerResult.Error.Message));

        var handlerResponse = handlerResult.Value;
        return Results.Created(
            $"/conversation/{handlerResponse.Conversation.Id}", 
            new CreateConversationEndpointResponse(handlerResponse.Conversation, handlerResponse.Participants));
    }
}

public record CreateConversationEndpointRequest(ConversationType Type, IEnumerable<Guid> Participants);
public record CreateConversationEndpointResponse(
    ConversationSummaryResponse Conversation,
    IEnumerable<UserSummaryResponse> Participants);

public class CreateConversationRequestValidator : AbstractValidator<CreateConversationEndpointRequest>
{
    public CreateConversationRequestValidator()
    {
        RuleFor(request => request.Participants).NotEmpty();
        RuleFor(request => request.Type).NotNull();
    }
}