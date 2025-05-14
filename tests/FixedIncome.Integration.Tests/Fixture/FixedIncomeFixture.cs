using FixedIncome.Application.Mediator;
using Microsoft.EntityFrameworkCore;
using FixedIncome.Infrastructure.Persistence;
using FixedIncome.Domain.FixedIncomeSimulation;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Integration.Tests.Fixture;

public class FixedIncomeFixture : IAsyncDisposable
{
    private readonly IServiceScope _scope;

    public FixedIncomeFixture()
    {
        WebAppFactory<Program> webAppFactory = new WebAppFactory<Program>();
        var serviceProvider = webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();
        
        _scope = serviceProvider.CreateScope();
        
        _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.EnsureDeleted();
        
        _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.Migrate();  
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
    
    public async Task<object?> SendCommand<TResponse>(IRequest<TResponse> command)
    {
        if (command is null)
            throw new ArgumentException("Command cannot be null");
        
        var mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        
        return await mediator.Send(command);
    }
}