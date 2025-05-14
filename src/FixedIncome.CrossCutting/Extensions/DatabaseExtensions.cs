using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using FixedIncome.Domain.FixedIncomeSimulation.Repositories;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.CrossCutting.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<PostgresConfiguration>();
            options.UseNpgsql(configuration.GetConnectionString());
        });

        services.AddTransient<IDapperDbContext, DapperDbContext>();
        services.AddScoped<IFixedIncomeRepository, FixedIncomeRepository>();
        services.AddScoped<IFixedIncomeQueryRepository, FixedIncomeQueryRepository>();
        
        return services;
    }
}