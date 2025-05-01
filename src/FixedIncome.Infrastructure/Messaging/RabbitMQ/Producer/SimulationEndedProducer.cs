using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;

public class SimulationEndedProducer : IProducer
{
    private readonly IModel _channel;

    public SimulationEndedProducer(IMessageBrokerConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish(object message)
    {
        var jsonObject = JsonSerializer.Serialize(message);
        var bodyMessage = Encoding.UTF8.GetBytes(jsonObject);
        
        _channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, body: bodyMessage);
    }
    public string QueueName => "fixed_income_simulation_created";
}