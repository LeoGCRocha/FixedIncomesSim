using RabbitMQ.Client;

namespace FixedIncome.Infrastructure.Messaging.Abstractions;

public interface IMessageBrokerConnection
{
    public IModel CreateModel();
}