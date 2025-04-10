using FixedIncome.Domain.FixedIncomeSimulation.Repository;

namespace FixedIncome.Domain.Common.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IFixedIncomeRepository FixedIncomeRepository { get; }

    public Task CommitAsync();
}