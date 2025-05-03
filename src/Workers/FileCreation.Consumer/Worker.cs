using FixedIncome.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileCreation.Consumer;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _provider;
    
    public Worker(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var consumer = _provider.GetRequiredService<IConsumer>();
            
            consumer.Consuming();
            
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}