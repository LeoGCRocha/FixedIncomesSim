using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using FixedIncome.Domain.FixedIncomeSimulation.Events;
using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;

public class SimulationCreatedProducer : IProducer
{
    private readonly IModel _channel;

    public SimulationCreatedProducer(IMessageBrokerConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish(FixedIncomeSimulationEnded message)
    {
        var jsonObject = JsonSerializer.Serialize(message);
        var bodyMessage = Encoding.UTF8.GetBytes(jsonObject);
        
        _channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, body: bodyMessage);
    }
    public string QueueName => "fixed_income_simulation_created";
}