using FixedIncome.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileCreation.Consumer;

public class Worker(IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = provider.CreateScope();
            var consumer = provider.GetRequiredService<IConsumer>();
            
            consumer.Consuming();
            
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}