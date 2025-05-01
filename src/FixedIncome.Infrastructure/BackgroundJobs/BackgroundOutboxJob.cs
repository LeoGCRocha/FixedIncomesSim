using FixedIncome.Application.Factories.Producer;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using FixedIncome.Infrastructure.Factories.Producer;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Infrastructure.Persistence.Outbox;
using Microsoft.Extensions.Logging;

namespace FixedIncome.Infrastructure.BackgroundJobs;

public class BackgroundOutboxJob : BackgroundService
{
    private const int StopTimeInSeconds = 10;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundOutboxJob> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public BackgroundOutboxJob(
        IServiceProvider provider,
        ILogger<BackgroundOutboxJob> logger, 
        IBackgroundTaskQueue backgroundTaskQueue)
    {
        _logger = logger;
        _serviceProvider = provider;
        _backgroundTaskQueue = backgroundTaskQueue;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try {
            while (!stoppingToken.IsCancellationRequested) {
                using var scope = _serviceProvider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var producerFactory = scope.ServiceProvider.GetRequiredService<IProducerFactory>();
                var producer = producerFactory.GetProducerService(ProducerType.SimulationEnded);
                var outboxMessages = await uow.OutboxPatternRepository.GetBatch(limit:10, offset:0);
                
                foreach (var outboxMessage in outboxMessages)
                {
                    outboxMessage.ProcessedOn = DateTime.Now;
                    try
                    {
                        if (outboxMessage.Type == OutboxMessageTypes.Email.ToString())
                        {
                            await _backgroundTaskQueue.EnqueueNewTask(async () =>
                            {
                                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                            }, stoppingToken);
                            
                            continue;
                        }

                        if (outboxMessage.Type != OutboxMessageTypes.File.ToString())
                            throw new Exception("Invalid OutboxMessageType");
                        
                        producer.Publish(outboxMessage.Content);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process outbox message {ARG0}", outboxMessage.Id);
                        outboxMessage.Error = ex.Message;
                    }
                }

                await uow.CommitAsync();
                await Task.Delay(TimeSpan.FromSeconds(StopTimeInSeconds), stoppingToken);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process background jobs");
        }
    }
}