using FixedIncome.Domain.Entities;
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
        builder.Property<DateTime>("created_at")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        
        builder.Property<DateTime>("updated_at")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}