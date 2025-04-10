using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FixedIncome.Domain.FixedIncomeSimulation;

namespace FixedIncome.Infrastructure.Persistence.FixedIncomeSimulation.Configurations;

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

        builder.HasMany<FixedIncomeOrderEvent>("_events")
            .WithOne()
            .HasForeignKey("FixedIncomeOrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<FixedIncomeSim>()
            .WithMany()
            .HasForeignKey("FixedIncomeId")
            .OnDelete(DeleteBehavior.Cascade);

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
        
        // Shadow properties
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        
        builder.Property<DateTime>("UpdatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}