using System.Reflection;
using FixedIncome.Application.Mediator;
using FixedIncome.Infrastructure.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.CrossCutting.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddBasicMediator(this IServiceCollection services, Assembly? assembly = null)
    {
        services.AddScoped<IMediator, Mediator>();
        
        assembly ??= Assembly.GetCallingAssembly();

        var genericHandlerType = typeof(IRequestHandler<,>);

        var handlerTypes = assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false, IsInterface: false })
            .SelectMany(
                type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericHandlerType)
                    .Select(i => new { Interface = i, Implementation = type })
            );

        foreach (var handler in handlerTypes)
            services.AddScoped(handler.Interface, handler.Implementation);
        
        
        return services;
    } 
}