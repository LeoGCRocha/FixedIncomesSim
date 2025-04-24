using System.Text;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ.Consumer;

public class SimulationCreatedConsumer : IConsumer
{
    private readonly IModel _channel;
    private readonly EventingBasicConsumer? _consumer;
    public string QueueName => "fixed_income_simulation_created";

    public SimulationCreatedConsumer(IConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        const ushort prefetchCount = 1; // Number of messages processed
        // Basic Qos is number of messages a consumer can receive without ack
        _channel.BasicQos(0, prefetchCount, false);

        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += Consume_Message;
    }

    private static void Consume_Message(object? _, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        
        Console.WriteLine(message);
    }
}