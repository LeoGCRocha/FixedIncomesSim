using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FixedIncome.Infrastructure.BackgroundJobs;

public class BackgroundJobs : BackgroundService
{
    private readonly ILogger<BackgroundJobs> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public BackgroundJobs(ILogger<BackgroundJobs> logger, IBackgroundTaskQueue backgroundTaskQueue)
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
                {
                    await workItem();
                }
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