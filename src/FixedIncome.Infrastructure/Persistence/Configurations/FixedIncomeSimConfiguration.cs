using Microsoft.EntityFrameworkCore;

using FixedIncome.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.Configurations;

public class FixedIncomeSimConfiguration : IEntityTypeConfiguration<FixedIncomeSim>
{
    public void Configure(EntityTypeBuilder<FixedIncomeSim> builder)
    {
        builder.ToTable("fixed_income_simulation");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.StartDate)
            .IsRequired();

        builder.Property(f => f.EndDate)
            .IsRequired();

        builder.Property(f => f.MonthlyContribution)
            .IsRequired();

        builder.Property(f => f.StartAmount)
            .HasDefaultValue(0);

        builder.Property(f => f.MonthlyYield)
            .IsRequired();

        builder
            .HasMany<FixedIncomeOrder>("_orders")
            .WithOne()
            .HasForeignKey("FixedIncomeId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<FixedIncomeBalance>("_balances") // Have many balances
            .WithOne() // Balance have one FixedIncome
            .HasForeignKey("FixedIncomeId")
            .OnDelete(DeleteBehavior.Cascade);
        
        // Shadow columns
        builder.Property<DateTime>("created_at")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        builder.Property<DateTime>("update_at")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}