namespace MessagingApp.Application.Interfaces;

public interface IMediator
{
    TResponse Send<TResponse>(IRequest<TResponse> request);
}