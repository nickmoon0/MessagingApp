using LanguageExt.Common;

namespace MessagingApp.Application.Common.Interfaces.Mediator;

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processes a request sent to a handler
    /// </summary>
    /// <param name="req">Request to process</param>
    /// <returns>Response object</returns>
    public Task<Result<TResponse>> Handle(TRequest req);
}
