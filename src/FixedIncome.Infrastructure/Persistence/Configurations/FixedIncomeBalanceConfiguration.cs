using FixedIncome.Domain.Entities;
using FixedIncome.Domain.FixedIncomes.FixedIncomeBalances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.Configurations;

public class FixedIncomeBalanceConfiguration : IEntityTypeConfiguration<FixedIncomeBalance>
{
    public void Configure(EntityTypeBuilder<FixedIncomeBalance> builder)
    {
        builder.ToTable("fixed_income_balance");

        builder.HasOne<FixedIncomeSim>()
            .WithMany()
            .HasForeignKey("FixedIncomeId") //  On delete FixedIncomeId
            .OnDelete(DeleteBehavior.Cascade); // Will delete relations with FixedIncomeBalance
        
        builder.Property(f => f.Amount)
            .IsRequired();

        builder.Property(f => f.Profit)
            .IsRequired();

        builder.Property<DateTime>("created_at")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        builder.Property<DateTime>("updated_at")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}