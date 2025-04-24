using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace FixedIncome.Integration.Tests.Fixture;

public class WebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.Sources.Clear();

            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                // Postgres
                ["Postgres:Host"] = "localhost",
                ["Postgres:Port"] = "5432",
                ["Postgres:Database"] = "fixed_income_test_db",
                ["Postgres:Username"] = "postgres",
                ["Postgres:Password"] = "postgres"
            });
        });
        
        builder.UseEnvironment("Testing");
    }
}