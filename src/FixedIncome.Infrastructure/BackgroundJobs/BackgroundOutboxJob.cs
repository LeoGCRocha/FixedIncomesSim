using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Infrastructure.Persistence.Abstractions;

namespace FixedIncome.Infrastructure.BackgroundJobs;

public class BackgroundOutboxJob : BackgroundService
{
    private const int StopTimeInSeconds = 10;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundOutboxJob(IServiceProvider provider)
    {
        _serviceProvider = provider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IOutboxPatternRepository>();
            var outboxMessages = await repository.GetBatch(limit:10, offset:0);
            
            foreach (var outboxMessage in outboxMessages)
            {
                // TODO Solve this
            }
            
            await Task.Delay(TimeSpan.FromSeconds(StopTimeInSeconds), stoppingToken);
        }
    }
}