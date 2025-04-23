using FixedIncome.Application.Factories;
using FixedIncome.Application.Factories.Producer;
using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation.Repository;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.DomainEvents;
using FixedIncome.Infrastructure.DomainEvents.Abstractions;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Infrastructure.Persistence.FixedIncomeSimulation;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FixedIncome.API.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFixedIncomeRepository, FixedIncomeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddSingleton<IConnection>(provider =>
        {
            var configuration = provider.GetRequiredService<IOptions<RabbitMqConfiguration>>();
            
            var factory = new ConnectionFactory
            {
                Uri = new Uri(configuration.Value.GetConnectionString())
            };
            
            return factory.CreateConnection();
        });

        services.AddSingleton<IMessageBrokerConnection, MessageBrokerConnection>();

        services.AddScoped<IProducer, SimulationCreatedProducer>();
        services.AddScoped<IProducerFactory, ProducerFactory>();

        return services;
    }
}