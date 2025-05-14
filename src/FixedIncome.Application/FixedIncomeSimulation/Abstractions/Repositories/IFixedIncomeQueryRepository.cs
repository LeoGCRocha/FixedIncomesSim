using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;

namespace FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;

public interface IFixedIncomeQueryRepository
{
    public Task<FixedIncomeResponse?> GetResultById(Guid id);
    public Task<IEnumerable<FixedBalanceResponse>> GetBalancesById(Guid id);
}