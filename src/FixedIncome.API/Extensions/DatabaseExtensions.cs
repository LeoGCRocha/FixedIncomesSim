using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.API.Extensions;

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
        
        return services;
    }
}