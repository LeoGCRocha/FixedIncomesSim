namespace FixedIncome.Domain.FixedIncomeSimulation.Repository;

public interface IFixedIncomeRepository
{
    public Task<FixedIncomeSim> AddAsync(FixedIncomeSim fixedIncomeToAdd);
    public Task DeleteAsync(Guid id);
}