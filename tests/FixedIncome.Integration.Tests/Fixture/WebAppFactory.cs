using NSubstitute;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Factories.Producer;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Integration.Tests.Fixture;

public class WebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly int _port;

    public WebAppFactory(int port)
    {
        _port = port;
    }

    public static Dictionary<string, string?> StringDictionary(int port)
    {
        return new Dictionary<string, string?>()
        {
            // Postgres
            ["Postgres:Host"] = "localhost",
            ["Postgres:Port"] = port.ToString(),
            ["Postgres:Database"] = "fixed_income_test_db",
            ["Postgres:Username"] = "postgres",
            ["Postgres:Password"] = "postgres",
        };
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(StringDictionary(_port));
        });

        builder.ConfigureServices((context, services) =>
        {
            var config = new PostgresConfiguration();
            context.Configuration.GetSection("Postgres").Bind(config);
            services.AddSingleton(config);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IConnection>();
            services.RemoveAll<IMessageBrokerConnection>();
            services.RemoveAll<SimulationEndedProducer>();
            services.RemoveAll<IProducerFactory>();

            services.AddSingleton(Substitute.For<IConnection>());
            services.AddSingleton(Substitute.For<IMessageBrokerConnection>());
            services.AddScoped<SimulationEndedProducer>();
            services.AddScoped<IProducerFactory, ProducerFactory>();
        });
        
        builder.UseEnvironment("Testing");
    }
}