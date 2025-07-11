using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.Configurations;

public class FixedIncomeOrderConfiguration : IEntityTypeConfiguration<FixedIncomeOrder>
{
    public void Configure(EntityTypeBuilder<FixedIncomeOrder> builder)
    {
        builder.ToTable("fixed_income_order");
        
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.StartDate)
            .IsRequired();

        builder.Property(f => f.EndDate)
            .IsRequired();

        builder.Property(f => f.StartAmount)
            .HasDefaultValue(0);

        builder.Property(f => f.CurrentAmount)
            .HasDefaultValue(0);

        builder.Property(f => f.Tax)
            .IsRequired();

        builder.Property(f => f.TaxGroup)
            .IsRequired();

        builder.Property(f => f.MonthlyYield)
            .IsRequired();

        builder.Ignore(b => b.GetEvents);
        
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        
        builder.Property<DateTime>("UpdatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();

        builder.HasMany<FixedIncomeOrderEvent>("_events")
            .WithOne()
            .HasForeignKey("FixedIncomeOrderId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}