using FixedIncome.Infrastructure.Persistence.Outbox;

namespace FixedIncome.Infrastructure.Persistence.Abstractions;

public interface IOutboxPatternRepository
{
    public Task AddAsync(OutboxMessage outboxMessage);
    public Task DeleteAsync(Guid id);
    public Task UpdateMessage(Guid id, OutboxMessage messageUpdated);
    public Task<IEnumerable<OutboxMessage>> GetBatch(int limit, int offset);
}