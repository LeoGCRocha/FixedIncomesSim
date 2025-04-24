using MediatR;
using FixedIncome.Domain.FixedIncomeSimulation.Events;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class SimulationEndedEventHandler : INotificationHandler<FixedIncomeSimulationEnded>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IProducer _producer;


    public SimulationEndedEventHandler(IBackgroundTaskQueue backgroundTaskQueue, IProducer producer)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _producer = producer;
    }

    public Task Handle(FixedIncomeSimulationEnded notification, CancellationToken cancellationToken)
    {
        // TODO Add maybe outbox pattern here
        _backgroundTaskQueue.EnqueueNewTask(async () =>
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            await Task.Delay(1000, cancellationToken);
            
            Console.WriteLine("Email was successfully sended.");
        }, cancellationToken);
        
        _producer.Publish(notification);
        
        return Unit.Task;
    }
}