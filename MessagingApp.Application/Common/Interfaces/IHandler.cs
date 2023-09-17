using LanguageExt.Common;

namespace MessagingApp.Application.Common.Interfaces;

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Result<TResponse> Handle(TRequest req);
}
