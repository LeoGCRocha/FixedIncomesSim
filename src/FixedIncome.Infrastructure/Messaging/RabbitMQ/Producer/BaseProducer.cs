using System.Text;
using System.Text.Json;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using RabbitMQ.Client;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;

public abstract class BaseProducer : IProducer
{
    private readonly IModel _channel;

    protected BaseProducer(IMessageBrokerConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }
    
    public void Publish(object message)
    {
        try
        {
            string jsonObject = message as string ?? JsonSerializer.Serialize(message);
            var bodyMessage = Encoding.UTF8.GetBytes(jsonObject);
            _channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, body: bodyMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    
    public abstract string QueueName { get; }
}
