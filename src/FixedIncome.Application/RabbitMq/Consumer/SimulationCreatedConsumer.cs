using MediatR;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using FixedIncome.Application.Factories.Producer;
using FixedIncome.Infrastructure.Factories.Producer;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;

namespace FixedIncome.Application.RabbitMq.Consumer;

public class SimulationCreatedConsumer : IConsumer
{
    private readonly IModel _channel;
    private readonly IMediator _mediator;
    private readonly EventingBasicConsumer? _consumer;
    private readonly IProducer _producer;
    public string QueueName => "fixed_income_simulation_created";

    public SimulationCreatedConsumer(IConnection connection, IMediator mediator, IProducerFactory producerFactory)
    {
        _mediator = mediator;
        _channel = connection.CreateModel();
        _producer = producerFactory.GetProducerService(ProducerType.Exception);
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

        try
        {
            var task = _mediator.Send(JsonConvert.DeserializeObject<CreateBalanceFileCommand>(message)!);
            task.GetAwaiter().GetResult();
        }
        catch(Exception ex)
        {
            _producer.Publish(ex.Message);
            return;
        }
        _channel.BasicAck(ea.DeliveryTag, false);
    }
}