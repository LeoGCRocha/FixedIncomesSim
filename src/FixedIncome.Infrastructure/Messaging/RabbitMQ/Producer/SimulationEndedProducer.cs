using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;

public class SimulationEndedProducer(IMessageBrokerConnection connection) : BaseProducer(connection)
{
    public override string QueueName => "fixed_income_simulation_created";
}