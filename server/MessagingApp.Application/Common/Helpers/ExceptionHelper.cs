using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Domain.Exceptions;

namespace MessagingApp.Application.Common.Helpers;

public static class ExceptionHelper
{
    public static Exception ResolveException(DomainException ex)
    {
        return ex switch
        {
            BadRequestException badReq => new InvalidOperationException(badReq.Message),
            NotFoundException notFound => new EntityNotFoundException(notFound.Message),
            UnauthorisedException unAuth => new UnauthorizedAccessException(unAuth.Message),
            _ => throw new Exception(ex.Message)
        };
    }
}