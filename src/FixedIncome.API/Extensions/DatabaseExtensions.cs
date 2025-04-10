using FixedIncome.API.Configuration;
using FixedIncome.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.API.Extensions;

public static class DatabaseExtensions
{
    public static WebApplicationBuilder AddDbServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var postgres = new PostgresConfiguration();
        configuration.GetSection("Postgres").Bind(postgres);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(postgres.GetConnectionString());
        });
        
        return builder;
    }
}