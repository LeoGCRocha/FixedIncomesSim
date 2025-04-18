using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.Repository;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Persistence.FixedIncomeSimulation;

public class FixedIncomeRepository : IFixedIncomeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FixedIncomeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public  async Task<FixedIncomeSim> AddAsync(FixedIncomeSim fixedIncomeToAdd)
    {
        await _dbContext.FixedIncomeSims.AddAsync(fixedIncomeToAdd);
        
        return fixedIncomeToAdd;
    }

    public async Task<FixedIncomeSim?> DeleteAsync(Guid id)
    {
        var incomeSim = await _dbContext.FixedIncomeSims.FirstOrDefaultAsync(f => f.Id == id);
        if (incomeSim is null) return incomeSim;
        
        _dbContext.FixedIncomeSims.Remove(incomeSim);
        return incomeSim;
    }
}