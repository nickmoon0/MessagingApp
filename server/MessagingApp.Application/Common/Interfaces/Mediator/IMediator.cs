using LanguageExt.Common;

namespace MessagingApp.Application.Common.Interfaces.Mediator;

public interface IMediator
{
    public Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request);
}