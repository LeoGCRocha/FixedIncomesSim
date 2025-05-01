using FixedIncome.Infrastructure.Factories.Producer;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Infrastructure.Messaging.Abstractions;
using FixedIncome.Infrastructure.Messaging.RabbitMQ.Producer;

namespace FixedIncome.Application.Factories.Producer;

public class ProducerFactory : IProducerFactory
{
    private readonly IServiceProvider _provider;
    private readonly Dictionary<ProducerType, Type> _producers;

    public ProducerFactory(IServiceProvider provider)
    {
        _provider = provider;
        _producers = new Dictionary<ProducerType, Type>()
        {
            { ProducerType.SimulationEnded, typeof(SimulationEndedProducer) }
        };
    }
    
    public IProducer GetProducerService(ProducerType type)
    {
        if (_producers.TryGetValue(type, out var producerType))
        {
            return (IProducer) _provider.GetRequiredService(producerType);
        }

        throw new ArgumentException("Invalid producer type");
    }
}   