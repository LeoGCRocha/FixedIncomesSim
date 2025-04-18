using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.FixedIncomeSimulation.Configurations;

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

        // TODO: Change this to can be null
        builder.OwnsOne(f => f.Information)
            .Property(i => i.Title)
            .HasColumnName("investiment_title")
            .HasDefaultValue(null);

        builder.OwnsOne(f => f.Information, info =>
        {
            info.Property(i => i.Type)
                .HasDefaultValue(null);

            info.HasIndex(f => f.Type);
        });
        
        builder.HasIndex(f => f.StartDate);
        
        // Shadow columns
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        builder.HasIndex("CreatedAt");

        builder.Property<DateTime>("UpdatedAt")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}