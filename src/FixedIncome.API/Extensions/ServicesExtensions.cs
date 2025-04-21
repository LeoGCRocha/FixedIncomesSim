using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation.Repository;
using FixedIncome.Infrastructure.DomainEvents;
using FixedIncome.Infrastructure.DomainEvents.Abstractions;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Infrastructure.Persistence.FixedIncomeSimulation;

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

        return services;
    }
}