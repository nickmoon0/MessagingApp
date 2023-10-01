using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Domain.Exceptions;

namespace MessagingApp.Application.Common.BaseClasses;

public abstract class BaseHandler<TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> 
{
    public async Task<Result<TResponse>> Handle(TRequest req)
    {
        try
        {
            return await HandleRequest(req);
        }
        catch (BadRequestException ex)
        {
            var returnEx = new InvalidOperationException(ex.Message);
            return new Result<TResponse>(returnEx);
        }
        catch (NotFoundException ex)
        {
            var returnEx = new EntityNotFoundException(ex.Message);
            return new Result<TResponse>(returnEx);
        }
        catch (UnauthorisedException ex)
        {
            var returnEx = new UnauthorizedAccessException(ex.Message);
            return new Result<TResponse>(returnEx);
        }
        catch (Exception ex)
        {
            return new Result<TResponse>(ex);
        }
    }

    protected abstract Task<Result<TResponse>> HandleRequest(TRequest request);
}