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
        var handlerType = typeof(IHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));
    
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)request);
    }
}