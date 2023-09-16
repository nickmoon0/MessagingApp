using MessagingApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingApp.Application;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        var handler = _serviceProvider.GetRequiredService<IHandler<IRequest<TResponse>, TResponse>>();
        return handler.Handle(request);
    }
}