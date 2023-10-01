using System.Security.Authentication;
using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MessagingApp.Api.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ToOk<TResult, TContract>(
        this Result<TResult> result, Func<TResult, TContract> mapper)
    {
        return result.Match<IActionResult>(obj =>
        {
            var response = mapper(obj);
            return new OkObjectResult(response);
        }, GetErrorActionResult);
    }
    public static IActionResult ToOk<TResult>(
        this Result<TResult> result)
    {
        return result.Match<IActionResult>(obj => new OkObjectResult(obj), GetErrorActionResult);
    }
    
    public static IActionResult ToCreated<TResult, TContract>(
        this Result<TResult> result, string location, Func<TResult, TContract> mapper)
    {
        return result.Match<IActionResult>(obj =>
        {
            var response = mapper(obj);
            return new CreatedResult(location, response);
        }, GetErrorActionResult);
    }
    
    public static IActionResult ToCreated<TResult>(
        this Result<TResult> result, string location)
    {
        return result.Match<IActionResult>(obj => new CreatedResult(location, obj), GetErrorActionResult);
    }


    private static IActionResult GetErrorActionResult(Exception ex)
    {
        return ex switch
        {
            AuthenticationException => CreateErrorResult(Status401Unauthorized, ex.Message),
            BadValuesException => CreateErrorResult(Status400BadRequest, ex.Message),
            EntityAlreadyExistsException => CreateErrorResult(Status409Conflict, ex.Message),
            EntityNotFoundException => CreateErrorResult(Status404NotFound, ex.Message),
            InvalidOperationException => CreateErrorResult(Status400BadRequest, ex.Message),
            MissingConfigException => CreateErrorResult(Status500InternalServerError, "Missing configuration."),
            UnauthorizedAccessException => CreateErrorResult(Status401Unauthorized, ex.Message),
            ValidationException => CreateErrorResult(Status400BadRequest, ex.Message),
            _ => CreateErrorResult(Status500InternalServerError, "Internal server error.")
        };
    }
    
    private static IActionResult CreateErrorResult(int statusCode, string errorMessage)
    {
        var errorResult = new ErrorResult
        {
            StatusCode = statusCode,
            ErrorMessage = errorMessage
        };
        return new JsonResult(errorResult) { StatusCode = statusCode };
    }

    public class ErrorResult
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = null!;
    }
}