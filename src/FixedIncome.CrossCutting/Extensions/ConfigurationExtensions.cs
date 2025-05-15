using FixedIncome.Application.FixedIncomeSimulation.Abstractions;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Factories.Producer;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ;

namespace FixedIncome.CrossCutting.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = new RabbitMqConfiguration();
        configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);
        services.AddSingleton(rabbitMqConfig);

        var postgresConfiguration = new PostgresConfiguration();
        configuration.GetSection("Postgres").Bind(postgresConfiguration);
        services.AddSingleton(postgresConfiguration);

        var pathsConfiguration = new PathsConfiguration();
        configuration.GetSection("Paths").Bind(pathsConfiguration);
        services.AddSingleton(pathsConfiguration);
        
        return services;
    }
    public static IServiceCollection AddMessaging(this IServiceCollection services) {
        // Here is a problem, if connection fails the connection will never be open again unless the application is restarted
        //  The responsibility of the creation of the connection should be replicated on the Producer
        services.AddSingleton<IConnection>(provider =>
        {
            var configuration = provider.GetRequiredService<RabbitMqConfiguration>();
            
            var factory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetConnectionString())
            };
                
            return factory.CreateConnection();
        });
        services.AddScoped<IProducerFactory, ProducerFactory>();
        services.AddSingleton<IMessageBrokerConnection, MessageBrokerConnection>();
        services.AddTransient<IPathProvider, PathProvider>();

        return services;
    }
}