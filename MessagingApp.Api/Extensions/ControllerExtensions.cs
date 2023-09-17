using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
    
    public static IActionResult ToCreated<TResult, TContract>(
        this Result<TResult> result, string location, Func<TResult, TContract> mapper)
    {
        return result.Match<IActionResult>(obj =>
        {
            var response = mapper(obj);
            return new CreatedResult(location, response);
        }, GetErrorActionResult);
    }

    private static IActionResult GetErrorActionResult(Exception ex)
    {
        return ex switch
        {
            ValidationException => new BadRequestResult(),
            EntityAlreadyExistsException => new ConflictResult(),
            UnauthorizedAccessException => new UnauthorizedResult(),
            MissingConfigException => new StatusCodeResult(500),
            _ => new StatusCodeResult(500)
        };
    }
}