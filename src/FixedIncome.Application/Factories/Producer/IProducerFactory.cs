using FixedIncome.Infrastructure.Messaging.Abstractions;

namespace FixedIncome.Application.Factories;

public interface IProducerFactory
{
    IProducer ProducerType(string type);
}