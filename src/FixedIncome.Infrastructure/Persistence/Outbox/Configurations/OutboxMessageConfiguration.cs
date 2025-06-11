using FixedIncome.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixedIncome.Infrastructure.Persistence.Outbox.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_message");

        builder.Property(f => f.Id)
            .IsRequired();
        
        builder.Property(f => f.Type)
            .IsRequired();

        builder.Property(f => f.OccuredOn)
            .HasDefaultValueSql("NOW()");

        builder.Property(f => f.Content)
            .HasColumnType("jsonb")
            .IsRequired();
        
        builder.Property(f => f.Error);

        builder.Property(f => f.ProcessedOn);
    }
}
