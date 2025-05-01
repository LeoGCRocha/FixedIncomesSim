using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Infrastructure.DomainEvents.Abstractions;
using FixedIncome.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
    public DbSet<FixedIncomeSim> FixedIncomeSims => Set<FixedIncomeSim>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("fixed_incomes");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
