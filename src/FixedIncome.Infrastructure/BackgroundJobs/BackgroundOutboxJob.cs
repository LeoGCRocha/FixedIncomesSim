using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using FixedIncome.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Infrastructure.Persistence.Outbox;
using FixedIncome.Infrastructure.Factories.Producer;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using FixedIncome.Infrastructure.Exceptions;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using Polly;
using Polly.CircuitBreaker;

namespace FixedIncome.Infrastructure.BackgroundJobs;

public class BackgroundOutboxJob : BackgroundService
{
    private const int StopTimeInSeconds = 10;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundOutboxJob> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ISyncPolicy _circuitBreakerPolicy;

    public BackgroundOutboxJob(
        IServiceProvider provider,
        ILogger<BackgroundOutboxJob> logger, 
        IBackgroundTaskQueue backgroundTaskQueue,
        ISyncPolicy circuitBreakerPolicy)
    {
        _logger = logger;
        _serviceProvider = provider;
        _backgroundTaskQueue = backgroundTaskQueue;
        _circuitBreakerPolicy = circuitBreakerPolicy;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var producerFactory = scope.ServiceProvider.GetRequiredService<IProducerFactory>();
                IProducer producer;

                try
                {
                    // On circuit breaker open will not run de execute async
                    producer = _circuitBreakerPolicy.Execute(_ => 
                        producerFactory.GetProducerService(ProducerType.SimulationEnded), 
                        stoppingToken);
                }
                catch (BrokenCircuitException ex)
                {
                    _logger.LogError(ex, "Circuit breaker is now open for messaging publishing.");
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to connect to producer om messaging publishing.");
                    continue;
                }
                
                var outboxMessages = (await uow.OutboxPatternRepository.GetPendingBatch(limit: 10, offset: 0))
                    .ToList();

                foreach (var outboxMessage in outboxMessages)
                {
                    outboxMessage.ProcessedOn = DateTime.Now;
                    try
                    {
                        if (!Enum.TryParse<EOutboxMessageTypes>(outboxMessage.Type, out var typeEnum))
                            throw new Exception("Invalid OutBoxMessageType");

                        switch (typeEnum)
                        {
                            case EOutboxMessageTypes.Email:
                                await _backgroundTaskQueue.EnqueueNewTask(
                                    async () => { await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); },
                                    stoppingToken);

                                continue;
                            case EOutboxMessageTypes.File:
                                producer.Publish(outboxMessage.Content);
                                continue;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process outbox message {ARG0}", outboxMessage.Id);
                        outboxMessage.Error = ex.Message;
                    }
                }

                if (outboxMessages.Count > 0)
                    await uow.CommitAsync();
            }
            
            await Task.Delay(TimeSpan.FromSeconds(StopTimeInSeconds), stoppingToken);
            
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process background jobs");
        }
    }
}