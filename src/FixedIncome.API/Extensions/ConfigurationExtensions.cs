using FixedIncome.Infrastructure.Configuration;

namespace FixedIncome.API.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));
        services.Configure<PostgresConfiguration>(configuration.GetSection("Postgres"));

        return services;
    }
}