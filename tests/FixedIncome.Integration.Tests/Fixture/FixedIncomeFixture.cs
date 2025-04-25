using MediatR;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Domain.FixedIncomeSimulation;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Integration.Tests.Fixture;

public class FixedIncomeFixture : IAsyncDisposable
{
    private readonly WebAppFactory<Program> _webAppFactory;
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly IServiceScope _scope;

    public FixedIncomeFixture()
    {
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

    public async Task CreateFixedIncome(FixedIncomeSim simulation)
    {
        var context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.AddAsync(simulation);
        await context.SaveChangesAsync();
    }
    
    public async Task<object?> SendCommand<T>(T command)
    {
        if (command is null)
            throw new ArgumentException("Command cannot be null");
        
        var mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        
        return await mediator.Send(command);
    }
}