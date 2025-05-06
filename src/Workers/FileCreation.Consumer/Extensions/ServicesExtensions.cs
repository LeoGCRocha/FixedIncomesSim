using FixedIncome.Application.RabbitMq.Consumer;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FileCreation.Consumer.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IConsumer, SimulationCreatedConsumer>();
        
        return services;
    }
}