using FluentValidation;
using MediatR;
using MessagingApp.Application.Common.Handlers;
using MessagingApp.Application.Users.Queries.RetrieveUser;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<RetrieveUserQuery>, ValidateRetrieveUserQuery>();
        return services;
    }
}