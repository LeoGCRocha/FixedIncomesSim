using FixedIncome.Domain.FixedIncomeSimulation.Repository;

namespace FixedIncome.Infrastructure.Persistence.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IFixedIncomeRepository FixedIncomeRepository { get; }
    IOutboxPatternRepository OutboxPatternRepository { get;  }
    
    public Task CommitAsync();
}