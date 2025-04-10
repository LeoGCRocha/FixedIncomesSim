using FixedIncome.Domain.Entities;
using FixedIncome.Domain.FixedIncomes.FixedIncomeOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.Configurations;

public class FixedIncomeOrderEventConfiguration : IEntityTypeConfiguration<FixedIncomeOrderEvent>
{
    public void Configure(EntityTypeBuilder<FixedIncomeOrderEvent> builder)
    {
        builder.ToTable("fixed_income_order_event");

        builder.HasKey(f => f.Id);

        builder.HasOne<FixedIncomeOrder>()
            .WithMany()
            .HasForeignKey("FixedIncomeId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(f => f.StartReferenceDate)
            .IsRequired();

        builder.Property(f => f.EndReferenceDate)
            .IsRequired();

        builder.Property(f => f.StartAmount)
            .IsRequired();

        builder.Property(f => f.MonthlyYield)
            .IsRequired();

        builder.Property(f => f.Amount)
            .HasDefaultValue(0);

        builder.Property(f => f.Profit)
            .HasDefaultValue(0);
    }
}