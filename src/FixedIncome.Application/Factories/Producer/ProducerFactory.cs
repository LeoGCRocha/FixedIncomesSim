using FixedIncome.Infrastructure.Messaging.Abstractions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Application.Factories.Producer;

public class ProducerFactory(IServiceProvider provider) : IProducerFactory
{
    public IProducer ProducerType(string type)
    {
        if (type == nameof(SimulationCreatedProducer))
            return provider.GetRequiredService<SimulationCreatedProducer>();

        throw new Exception("Invalid producer type");
    }
}