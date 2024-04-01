using MessagingApp.Domain.Common;

namespace MessagingApp.Application.Common;

public interface IHandler<in TRequest, TResponse> where TResponse : class
{
    public Task<Result<TResponse>> Handle(TRequest request);
}