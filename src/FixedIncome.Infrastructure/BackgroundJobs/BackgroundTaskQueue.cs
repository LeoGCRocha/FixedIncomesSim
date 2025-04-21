using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;

namespace FixedIncome.Infrastructure.BackgroundJobs;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<Task>> _queueItems;
    private readonly ILogger<BackgroundTaskQueue> _logger;


    public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger)
    {
        _queueItems = Channel.CreateUnbounded<Func<Task>>();
        _logger = logger;
    }
    
    public async Task EnqueueNewTask(Func<Task> workerItem, CancellationToken cancellationToken = default)
    {
        if (workerItem is null)
            throw new ArgumentException("Invalid function pass as parameter to background job");

        try
        {
            await _queueItems.Writer.WriteAsync(workerItem, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Task enqueue operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on enqueue task {ARGS}", ex.Message);
        }
        
    }

    public async Task<Func<Task>?> DequeueTask(CancellationToken cancellationToken)
    {
        try
        {
            var workerItem = await _queueItems.Reader.ReadAsync(cancellationToken);
            return workerItem;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Task dequeue operation was cancelled.");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on dequeue task {ARGS}", ex.Message);
            return null;
        }
    }
}