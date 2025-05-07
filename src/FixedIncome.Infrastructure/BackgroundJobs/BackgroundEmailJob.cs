using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;

namespace FixedIncome.Infrastructure.BackgroundJobs;

public class BackgroundEmailJob : BackgroundService
{
    private readonly ILogger<BackgroundEmailJob> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public BackgroundEmailJob(ILogger<BackgroundEmailJob> logger, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _logger = logger;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workItem = await _backgroundTaskQueue.DequeueTask(stoppingToken);

                if (workItem != null)
                    await workItem();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Background worker canceled.");
                break; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing background task.");
            }
        }

        _logger.LogInformation("Background worker stopped.");
    }
}