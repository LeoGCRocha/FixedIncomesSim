using System.Text;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
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

    public SimulationCreatedConsumer(IConnection connection, IMediator mediator)
    {
        _mediator = mediator;
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

        try
        {
            var task = _mediator.Send(JsonConvert.DeserializeObject<CreateBalanceFileCommand>(message)!);
            task.GetAwaiter();
        }
        catch(Exception ex)
        {
            // This approach have a problem of message always go to the top of the queue
            // Better solution is sending to a DLQ  publish message eagain with some delay
            // Other solution is using headers on the message to count the number of attempts
            Console.WriteLine(ex.Message);
            _channel.BasicNack(ea.DeliveryTag, false, true);
            return;
        }
                    
        _channel.BasicAck(ea.DeliveryTag, false);
        
    }
}