using FixedIncome.Application.Factories.Producer;
using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Infrastructure.Factories.Producer;

public interface IProducerFactory
{
    IProducer GetProducerService(ProducerType type);
}