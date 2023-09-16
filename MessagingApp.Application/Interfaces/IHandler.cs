namespace MessagingApp.Application.Interfaces;

public interface IHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
{
    public TResponse Handle(TRequest req);
}
