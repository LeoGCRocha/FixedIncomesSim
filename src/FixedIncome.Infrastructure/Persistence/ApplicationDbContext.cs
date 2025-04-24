using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Infrastructure.DomainEvents.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FixedIncome.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
    public DbSet<FixedIncomeSim> FixedIncomeSims => Set<FixedIncomeSim>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("fixed_incomes");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEvents();
        
        return result;
    }

    private async Task PublishDomainEvents()
    {
        var domainEvents = ChangeTracker.Entries<FixedIncomeSim>()
            .SelectMany(ag => ag.Entity.GetDomainEvents())
            .ToList();

        await domainEventDispatcher.DispatchEvents(domainEvents);

        foreach (var entity in ChangeTracker.Entries<FixedIncomeSim>())
        {
            entity.Entity.ClearDomainEvents();
        }
    }
}
