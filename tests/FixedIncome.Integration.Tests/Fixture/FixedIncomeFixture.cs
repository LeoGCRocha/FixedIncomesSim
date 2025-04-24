using FixedIncome.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Integration.Tests.Fixture;

public class FixedIncomeFixture : IAsyncDisposable
{
    private readonly WebAppFactory<Program> _webAppFactory;
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly IServiceScope _scope;

    public FixedIncomeFixture()
    {
        Console.WriteLine("ok");
        _webAppFactory = new WebAppFactory<Program>();
        _serviceProvider = _webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();
        _scope = _serviceProvider.CreateScope();

        _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.EnsureDeleted();
        
        _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.EnsureCreated();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.EnsureDeletedAsync();
    }
}