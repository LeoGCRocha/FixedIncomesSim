namespace FixedIncome.Application.FixedIncomeSimulation.Abstractions.Outbox;

public class OutboxFileMessage
{
    public required Guid Id { get; init; }
}