using System.Net.Mime;
using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MessagingApp.Api.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
        
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("An invalid operation was performed");
            _logger.LogWarning(ex, ex.Message);

            // Set response details
            var details = new ProblemDetails
            {
                Status = Status400BadRequest,
                Type = "Bad Request",
                Title = "Bad Request",
                Detail = "A malformed or unexpected request was received"
            };
            var json = JsonSerializer.Serialize(details);
            
            // Set HTTP headers
            context.Response.StatusCode = Status400BadRequest;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            
            // Write response to body
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError("An unknown exception has occurred");
            _logger.LogError(ex, ex.Message);
            
            // Set response details
            var details = new ProblemDetails
            {
                Status = Status500InternalServerError,
                Type = "Internal Server Error",
                Title = "Internal Server Error",
                Detail = "An unexpected internal server error has occurred"
            };
            var json = JsonSerializer.Serialize(details);
            
            // Set HTTP headers
            context.Response.StatusCode = Status500InternalServerError;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            
            // Write response to body
            await context.Response.WriteAsync(json);
        }
    }
}