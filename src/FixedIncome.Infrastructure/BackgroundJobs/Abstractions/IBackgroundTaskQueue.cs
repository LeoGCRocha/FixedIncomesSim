namespace FixedIncome.Infrastructure.BackgroundJobs.Abstractions;

public interface IBackgroundTaskQueue
{
    public Task EnqueueNewTask(Func<Task> workerItem, CancellationToken cancellationToken = default);
    public Task<Func<Task>?> DequeueTask(CancellationToken cancellationToken);
}