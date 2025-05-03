using FixedIncome.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}