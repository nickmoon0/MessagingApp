using MessagingApp.Domain.Exceptions;
using FluentValidation.Results;
using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Services;

public static class ValidationErrorService
{
    /// <summary>
    /// Maps exception constants into exception objects to throw
    /// </summary>
    /// <param name="result"></param>
    /// <returns>A domain exception which can be thrown</returns>
    public static DomainException GetException(ValidationResult result)
    {
        // Result shouldn't be valid if it gets passed to this function
        if (result.IsValid) return new InternalServerErrorException("Internal Server Error", ErrorCodes.InternalServerError);
        
        var message = result.Errors.First().ErrorMessage;
        var code = result.Errors.First().ErrorCode;
        
        return code switch
        {
            ErrorCodes.BadRequest => new BadRequestException(message, code),
            ErrorCodes.Unauthorised => new UnauthorisedException(message, code),
            ErrorCodes.NotFound => new NotFoundException(message, code),
            _ => new InternalServerErrorException("Internal Server Error", ErrorCodes.InternalServerError)
        };
    }
    
}