using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Outbox;
using FixedIncome.Infrastructure.Persistence.Outbox;

namespace FixedIncome.Application.Factories.Outbox
{
    public interface IOutboxFactory
    {
        public OutboxMessage CreateOutboxMessage(OutboxMessageTypes type, Guid id);
    }
}