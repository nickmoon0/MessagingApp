using MessagingApp.Api.Common;
using MessagingApp.Api.Endpoints.Authentication;
using MessagingApp.Api.Endpoints.FriendRequests;
using MessagingApp.Api.Endpoints.User;

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
            .WithTags("Authentication Actions")
            .AllowAnonymous()
            .MapEndpoint<RegisterUserEndpoint>()
            .MapEndpoint<LoginUserEndpoint>()
            .MapEndpoint<RenewTokenEndpoint>();

        endpoints.MapGroup("/friendRequest")
            .WithTags("Friend Request Actions")
            .MapEndpoint<RespondToFriendRequestEndpoint>()
            .MapEndpoint<GetFriendRequestsEndpoint>();
        
        endpoints.MapGroup("/user")
            .WithTags("User Actions")
            .MapEndpoint<SendFriendRequestEndpoint>()
            .MapEndpoint<GetFriendsEndpoint>();
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}