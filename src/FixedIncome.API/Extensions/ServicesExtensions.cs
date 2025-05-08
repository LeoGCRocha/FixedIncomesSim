using FixedIncome.Application.Abstractions;
using FixedIncome.Application.Factories.Outbox;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.DomainEvents;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using FixedIncome.Application.Outbox;
using FixedIncome.Domain.FixedIncomeSimulation.Repositories;
using FixedIncome.Infrastructure.Persistence.Outbox;
using FixedIncome.Infrastructure.DomainEvents.Abstractions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Infrastructure.Persistence.Repositories;

namespace FixedIncome.API.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFixedIncomeRepository, FixedIncomeRepository>();
        services.AddScoped<IFixedIncomeQueryRepository, FixedIncomeQueryRepository>();
        services.AddScoped<IOutboxPatternRepository, OutboxPatternRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPathProvider, PathProvider>();
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IOutboxFactory, OutboxFactory>();
        services.AddScoped<SimulationEndedProducer>();

        return services;
    }
}