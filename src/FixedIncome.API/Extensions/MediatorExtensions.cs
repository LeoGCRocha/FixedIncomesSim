using System.Reflection;
using FixedIncome.Application.Mediator;
using FixedIncome.Infrastructure.Mediator;

namespace FixedIncome.API.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddBasicMediator(this IServiceCollection services, Assembly? assembly = null)
    {
        services.AddScoped<IMediator, Mediator>();
        
        // Get current context of assembly
        assembly ??= Assembly.GetCallingAssembly();

        var genericHandlerType = typeof(IRequestHandler<,>);

        var handlerTypes = assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface)
            .SelectMany(
                type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericHandlerType)
                    .Select(i => new { Interface = i, Implementation = type })
            );

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.Interface, handler.Implementation);
        }
        
        return services;
    } 
}