using System.Text;
using System.Text.Json;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using RabbitMQ.Client;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;

public class SimulationCreatedProducer
{
    private IModel _channel;

    public SimulationCreatedProducer(IMessageBrokerConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: "fixed_income_simulation_created", durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish(object message)
    {
        var jsonObject = JsonSerializer.Serialize<object>(message);
        var bodyMessage = Encoding.UTF8.GetBytes(jsonObject);
        
        _channel.BasicPublish(exchange: string.Empty, routingKey: "fixed_income_simulation_created", body: bodyMessage);
    }
}