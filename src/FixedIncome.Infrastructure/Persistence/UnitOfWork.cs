using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation.Repository;

namespace FixedIncome.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IFixedIncomeRepository FixedIncomeRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext, IFixedIncomeRepository fixedIncomeRepository)
    {
        _dbContext = dbContext;
        FixedIncomeRepository = fixedIncomeRepository;
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