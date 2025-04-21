using MediatR;
using FixedIncome.Domain.FixedIncomeSimulation.Events;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class SimulationEndedEventHandler : INotificationHandler<FixedIncomeSimulationEnded>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public SimulationEndedEventHandler(IBackgroundTaskQueue backgroundTaskQueue)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public Task Handle(FixedIncomeSimulationEnded notification, CancellationToken cancellationToken)
    {
        _backgroundTaskQueue.EnqueueNewTask(async () =>
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            await Task.Delay(1000, cancellationToken);
            
            Console.WriteLine("Email was successfully sended.");
        }, cancellationToken);
        
        return Unit.Task;
    }
}