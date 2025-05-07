namespace FixedIncome.Infrastructure.Messaging.Abstractions;

public interface IConsumer
{
    public string QueueName { get; }
    public void Consuming();
}