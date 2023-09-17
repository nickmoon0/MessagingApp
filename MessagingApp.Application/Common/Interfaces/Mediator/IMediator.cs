using LanguageExt.Common;

namespace MessagingApp.Application.Common.Interfaces.Mediator;

public interface IMediator
{
    Result<TResponse> Send<TResponse>(IRequest<TResponse> request);
}