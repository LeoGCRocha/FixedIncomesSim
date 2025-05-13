using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.Configurations;

public class FixedIncomeBalanceConfiguration : IEntityTypeConfiguration<FixedIncomeBalance>
{
    public void Configure(EntityTypeBuilder<FixedIncomeBalance> builder)
    {
        builder.ToTable("fixed_income_balance");

        builder.HasOne<Domain.FixedIncomeSimulation.FixedIncomeSim>()
            .WithMany()
            .HasForeignKey("FixedIncomeId") 
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(); 
        
        builder.Property(f => f.Amount)
            .IsRequired();

        builder.Property(f => f.Profit)
            .IsRequired();

        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        builder.Property<DateTime>("UpdatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}