using RabbitMQ.Client;
using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ;

public class MessageBrokerConnection : IMessageBrokerConnection
{
    private readonly IConnection _connection;
    
    public MessageBrokerConnection(IConnection connection)
    {
        _connection = connection ?? throw new InvalidOperationException("Connection should be exist at this part");
    }

    public IModel CreateModel()
    {
        return _connection.CreateModel();
    }
}