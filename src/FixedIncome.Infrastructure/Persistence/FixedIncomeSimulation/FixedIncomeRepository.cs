using System.Diagnostics;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FixedIncome.Infrastructure.Persistence.FixedIncomeSimulation;

public class FixedIncomeRepository : IFixedIncomeRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<FixedIncomeRepository> _logger;

    public FixedIncomeRepository(ApplicationDbContext dbContext, ILogger<FixedIncomeRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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