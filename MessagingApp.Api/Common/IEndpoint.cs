namespace MessagingApp.Api.Common;

public interface IEndpoint
{
    public static abstract void Map(IEndpointRouteBuilder app);
}