namespace FixedIncome.Application.FixedIncomeSimulation.Abstractions.Outbox;

public class OutboxEmailMessage
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Message { get; init; }
}