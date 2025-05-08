namespace FixedIncome.Infrastructure.Persistence.Outbox;

public class OutboxFileMessage
{
    public required Guid Id { get; init; }
}