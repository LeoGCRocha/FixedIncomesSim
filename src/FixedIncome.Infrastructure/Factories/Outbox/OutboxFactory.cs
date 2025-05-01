using FixedIncome.Application.Factories.Outbox;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Outbox;
using FixedIncome.Infrastructure.Persistence.Outbox;
using Newtonsoft.Json;

namespace FixedIncome.Infrastructure.Factories.Outbox;

public class OutboxFactory : IOutboxFactory
{

    public OutboxMessage CreateOutboxMessage(OutboxMessageTypes type, Guid id)
    {
        return type switch
        {
            OutboxMessageTypes.Email => OutboxMessage.OutboxMessageBuilder(OutboxMessageTypes.Email.ToString(),
                JsonConvert.SerializeObject(new OutboxEmailMessage
                {
                    Id = id,
                    Email = "defautl@gmail.com",
                    Message = $"Fixed Income with Id {id} was sent to process."
                })),
            OutboxMessageTypes.File => OutboxMessage.OutboxMessageBuilder(OutboxMessageTypes.File.ToString(),
                JsonConvert.SerializeObject(new OutboxFileMessage { Id = id })),
            _ => throw new Exception("Invalid type definition")
        };
    }
}