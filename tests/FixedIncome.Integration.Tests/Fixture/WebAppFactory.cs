using NSubstitute;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using FixedIncome.Application.Factories.Producer;
using FixedIncome.Infrastructure.BackgroundJobs;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using FixedIncome.Infrastructure.Factories.Producer;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Integration.Tests.Fixture;

public class WebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                // Postgres
                ["Postgres:Host"] = "localhost",
                ["Postgres:Port"] = "5432",
                ["Postgres:Database"] = "fixed_income_test_db",
                ["Postgres:Username"] = "postgres",
                ["Postgres:Password"] = "postgres",
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IConnection>();
            services.RemoveAll<IMessageBrokerConnection>();
            services.RemoveAll<SimulationEndedProducer>();
            services.RemoveAll<IProducerFactory>();

            services.AddSingleton<IConnection>(Substitute.For<IConnection>());
            services.AddSingleton<IMessageBrokerConnection>(Substitute.For<IMessageBrokerConnection>());
            services.AddScoped<SimulationEndedProducer>();
            services.AddScoped<IProducerFactory, ProducerFactory>();

            services.RemoveAll<IBackgroundTaskQueue>();
            services.RemoveAll<BackgroundOutboxJob>();
            services.RemoveAll<BackgroundEmailJob>();
        });
        
        builder.UseEnvironment("Testing");
    }
}