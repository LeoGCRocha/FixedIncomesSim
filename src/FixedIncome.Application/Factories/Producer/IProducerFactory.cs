using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Application.Factories.Producer;

public interface IProducerFactory
{
    IProducer GetProducerService(ProducerType type);
}