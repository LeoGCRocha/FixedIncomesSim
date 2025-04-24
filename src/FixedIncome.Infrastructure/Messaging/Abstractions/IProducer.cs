using FixedIncome.Domain.FixedIncomeSimulation.Events;

namespace FixedIncome.Infrastructure.Messaging.Abstractions;

public interface IProducer
{
    public void Publish(FixedIncomeSimulationEnded message);
    public string QueueName { get; }
}