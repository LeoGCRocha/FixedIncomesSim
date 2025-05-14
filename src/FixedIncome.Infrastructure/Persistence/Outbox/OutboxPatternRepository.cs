using FixedIncome.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using FixedIncome.Infrastructure.Persistence.Abstractions;

namespace FixedIncome.Infrastructure.Persistence.Outbox;

public class OutboxPatternRepository : IOutboxPatternRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OutboxPatternRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(OutboxMessage outboxMessage)
    {
        await _dbContext.OutboxMessages.AddAsync(outboxMessage);
    }

    public async Task DeleteAsync(Guid id)
    {
        var outboxMessage = await _dbContext.OutboxMessages.FirstOrDefaultAsync(f => f.Id == id);
        if (outboxMessage is null)
            return;
        _dbContext.Remove(outboxMessage);
    }

    public async Task UpdateMessage(Guid id, OutboxMessage messageUpdated)
    {
        var outboxMessage = await _dbContext.OutboxMessages.FirstOrDefaultAsync(f => f.Id == id);
        if (outboxMessage is null)
            return;
        outboxMessage.ProcessedOn = DateTime.Now;
        if (messageUpdated.Error is not null)
            outboxMessage.Error = messageUpdated.Error;
        _dbContext.Update(messageUpdated);
    }
    public async Task<IEnumerable<OutboxMessage>> GetPendingBatch(int limit, int offset)
    {
        return await _dbContext.OutboxMessages.Where(o => o.ProcessedOn == null).OrderBy(o => o.OccuredOn).Skip(offset).Take(limit).ToListAsync();
    }
}