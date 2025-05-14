namespace FixedIncome.Application.Outbox;

public sealed class OutboxMessage
{
    public required Guid Id { get; set; }
    public required string Type { get; init; }
    public required DateTime OccuredOn { get; init; }
    public required string Content { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }

    public static OutboxMessage OutboxMessageBuilder(string type, string content)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Content = content,
            OccuredOn = DateTime.Now
        };
    }
}