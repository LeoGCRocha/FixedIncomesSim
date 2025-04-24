using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FixedIncome.API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDbServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IOptions<PostgresConfiguration>>();
            options.UseNpgsql(configuration.Value.GetConnectionString());
        });

        services.AddTransient<IDapperDbContext, DapperDbContext>();
        
        return services;
    }
}