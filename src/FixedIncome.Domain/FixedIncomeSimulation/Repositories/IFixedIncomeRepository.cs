namespace FixedIncome.Domain.FixedIncomeSimulation.Repositories;

public interface IFixedIncomeRepository
{
    public Task<FixedIncomeSim> AddAsync(FixedIncomeSim fixedIncomeToAdd);
    public Task DeleteAsync(Guid id);
}