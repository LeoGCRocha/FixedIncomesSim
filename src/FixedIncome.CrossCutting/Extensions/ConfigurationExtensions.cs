using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Infrastructure.Configuration;
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
        
        return services;
    }
    public static IServiceCollection AddMessaging(this IServiceCollection services) {
        services.AddSingleton<IConnection>(provider =>
        {
            var configuration = provider.GetRequiredService<RabbitMqConfiguration>();
            
            var factory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetConnectionString())
            };
                
            return factory.CreateConnection();
        });
        services.AddSingleton<IMessageBrokerConnection, MessageBrokerConnection>();

        return services;
    }
}