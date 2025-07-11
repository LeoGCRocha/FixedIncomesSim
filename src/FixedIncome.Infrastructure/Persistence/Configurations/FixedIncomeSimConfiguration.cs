using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using Microsoft.EntityFrameworkCore;
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

        builder.Property(f => f.FinalGrossAmount)
            .HasDefaultValue(0);

        builder.Property(f => f.FinalNetAmount)
            .HasDefaultValue(0);

        builder.Ignore(b => b.GetBalances);
        builder.Ignore(b => b.GetOrders);
        builder.Ignore(b => b.GetOrderEvents);
        
        builder.HasMany<FixedIncomeOrder>("_orders")
            .WithOne()
            .HasForeignKey("FixedIncomeSimId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<FixedIncomeBalance>("_balances") 
            .WithOne() 
            .HasForeignKey("FixedIncomeSimId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(f => f.Information)
            .Property(i => i.Title)
            .HasColumnName("InvestmentTitle")
            .HasDefaultValue(null);

        builder.OwnsOne(f => f.Information)
            .Property(i => i.Type)
            .HasColumnName("InformationType")
            .HasDefaultValue(null);

        builder.HasIndex(f => f.StartDate);
        
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        builder.HasIndex("CreatedAt");

        builder.Property<DateTime>("UpdatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}