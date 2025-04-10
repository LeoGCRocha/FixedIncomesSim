using Microsoft.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Domain.FixedIncomeSimulation.FixedIncomeSim> FixedIncomeSims => Set<Domain.FixedIncomeSimulation.FixedIncomeSim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("fixed_incomes");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}