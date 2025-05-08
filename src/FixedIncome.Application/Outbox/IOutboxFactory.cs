using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Outbox;
using FixedIncome.Application.Outbox;
using FixedIncome.Infrastructure.Persistence.Outbox;

namespace FixedIncome.Application.Factories.Outbox
{
    public interface IOutboxFactory
    {
        public OutboxMessage CreateOutboxMessage(EOutboxMessageTypes type, Guid id);
    }
}