using MessagingApp.Api.Common;
using MessagingApp.Api.Endpoints.Authentication;

namespace MessagingApp.Api.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app
            .MapGroup("")
            .RequireAuthorization()
            .WithOpenApi();

        endpoints.MapGroup("/auth")
            .WithTags("Authentication")
            .AllowAnonymous()
            .MapEndpoint<RegisterUserEndpoint>();
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}