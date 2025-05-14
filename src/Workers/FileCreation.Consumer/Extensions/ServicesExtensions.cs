using FixedIncome.Infrastructure.Messaging.Abstractions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Consumer;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace FileCreation.Consumer.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ExceptionToDlqProducer>();
        services.AddScoped<IConsumer, SimulationCreatedConsumer>();
        
        return services;
    }
}