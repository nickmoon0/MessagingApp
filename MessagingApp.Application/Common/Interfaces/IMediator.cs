namespace MessagingApp.Application.Common.Interfaces;

public interface IMediator
{
    TResponse Send<TResponse>(IRequest<TResponse> request);
}