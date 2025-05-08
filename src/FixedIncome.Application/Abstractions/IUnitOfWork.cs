using FixedIncome.Domain.FixedIncomeSimulation.Repositories;
using FixedIncome.Infrastructure.Persistence.Abstractions;

namespace FixedIncome.Application.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IFixedIncomeRepository FixedIncomeRepository { get; }
    IOutboxPatternRepository OutboxPatternRepository { get;  }
    
    public Task CommitAsync();
}