using MessagingApp.Api.Endpoints;
using MessagingApp.Api.Filters;

namespace MessagingApp.Api.Extensions;

public static class EndpointFilterExtension
{
    /// <summary>
    /// Used to simplify the addition of request validation. Reduces need to specify RequestValidationFilter and attaches
    /// validation problem response.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <returns></returns>
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        builder
            .AddEndpointFilter<RequestValidationFilter<TRequest>>()
            .ProducesValidationProblem();

        return builder;
    }
}