﻿using LanguageExt.Common;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Common.Interfaces.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingApp.Application.Common;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));
    
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return await handler.Handle((dynamic)request);
    }
}