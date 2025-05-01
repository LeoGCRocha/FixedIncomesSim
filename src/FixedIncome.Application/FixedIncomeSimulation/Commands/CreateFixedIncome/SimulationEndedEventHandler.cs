// using MediatR;
// using FixedIncome.Application.Factories.Producer;
// using FixedIncome.Domain.FixedIncomeSimulation.Events;
// using FixedIncome.Infrastructure.Messaging.Abstractions;
// using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
//
// namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;
//
// public class SimulationEndedEventHandler : INotificationHandler<FixedIncomeSimulationEnded>
// {
//     private readonly IBackgroundTaskQueue _backgroundTaskQueue;
//     // TODO And UoW;
//     private readonly IProducer _producer;
//     public SimulationEndedEventHandler(IBackgroundTaskQueue backgroundTaskQueue, IProducerFactory factory)
//     {
//         _backgroundTaskQueue = backgroundTaskQueue;
//         _producer = factory.GetProducerService(ProducerType.SimulationEnded);
//     }
//
//     public Task Handle(FixedIncomeSimulationEnded notification, CancellationToken cancellationToken)
//     {
//         // TODO Add maybe outbox pattern here
//         _backgroundTaskQueue.EnqueueNewTask(async () =>
//         {
//             if (cancellationToken.IsCancellationRequested)
//                 return;
//
//             await Task.Delay(1000, cancellationToken);
//             
//             Console.WriteLine("Email was successfully sent.");
//         }, cancellationToken);
//         
//         _producer.Publish(notification);
//         
//         return Unit.Task;
//     }
// }