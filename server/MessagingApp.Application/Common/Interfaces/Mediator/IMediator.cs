using LanguageExt.Common;

namespace MessagingApp.Application.Common.Interfaces.Mediator;

public interface IMediator
{
    /// <summary>
    /// Sends a request to a handler
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request);
}