using FluentValidation;

namespace MessagingApp.Api.Filters;

public class RequestValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argToValidate = context.GetArgument<T>(0);
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        // If validator hasn't been applied to endpoint, skip filter
        if (validator is null) return await next.Invoke(context);
        
        // Check if request is valid
        var validationResult = await validator.ValidateAsync(argToValidate!);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        // Otherwise invoke the next filter in the pipeline
        return await next.Invoke(context);
    }
}