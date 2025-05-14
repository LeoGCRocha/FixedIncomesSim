using Microsoft.EntityFrameworkCore;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.Repositories;

namespace FixedIncome.Infrastructure.Persistence.Repositories;

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

    public async Task DeleteAsync(Guid id)
    {
        var incomeSim = await _dbContext.FixedIncomeSims.FirstOrDefaultAsync(f => f.Id == id);
        if (incomeSim is null) 
            return;
        
        _dbContext.FixedIncomeSims.Remove(incomeSim);
    }
}