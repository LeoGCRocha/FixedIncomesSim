using System.Text;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FixedIncome.Application.RabbitMq.Consumer;

public class SimulationCreatedConsumer : IConsumer
{
    private readonly IModel _channel;
    private readonly EventingBasicConsumer? _consumer;
    public string QueueName => "fixed_income_simulation_created";
    private readonly IMediator _mediator;

    public SimulationCreatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

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

    public void Consuming()
    {
        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: _consumer);
    }
    
    private void Consume_Message(object? _, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        var task = _mediator.Send(JsonConvert.DeserializeObject<CreateBalanceFileCommand>(message)!);

        task.GetAwaiter();
        
        _channel.BasicAck(ea.DeliveryTag, false);
    }
}