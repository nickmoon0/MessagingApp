using LanguageExt.Common;

namespace MessagingApp.Application.Common.Interfaces.Mediator;

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Task<Result<TResponse>> Handle(TRequest req);
}
