namespace FixedIncome.Infrastructure.Messaging.Abstractions;

public interface IProducer
{
    public void Publish(object message);
    public string QueueName { get; }
}