using FixedIncome.Domain.FixedIncomeSimulation.Repository;
using FixedIncome.Infrastructure.Persistence.Abstractions;

namespace FixedIncome.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IFixedIncomeRepository FixedIncomeRepository { get; }
    public IOutboxPatternRepository OutboxPatternRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext, IFixedIncomeRepository fixedIncomeRepository, IOutboxPatternRepository outboxPatternRepository)
    {
        _dbContext = dbContext;
        FixedIncomeRepository = fixedIncomeRepository;
        OutboxPatternRepository = outboxPatternRepository;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}