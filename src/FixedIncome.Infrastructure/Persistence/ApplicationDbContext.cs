using FixedIncome.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<FixedIncomeSim> FixedIncomeSims => Set<FixedIncomeSim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("fixed_incomes");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}