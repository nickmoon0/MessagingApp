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
            AuthenticationException => new UnauthorizedResult(),
            BadValuesException => new BadRequestResult(),
            EntityAlreadyExistsException => new ConflictResult(),
            EntityNotFoundException => new NotFoundResult(),
            InvalidOperationException => new BadRequestResult(),
            MissingConfigException => new StatusCodeResult(Status500InternalServerError),
            UnauthorizedAccessException => new UnauthorizedResult(),
            ValidationException => new BadRequestResult(),
            _ => new StatusCodeResult(Status500InternalServerError)
        };
    }
}