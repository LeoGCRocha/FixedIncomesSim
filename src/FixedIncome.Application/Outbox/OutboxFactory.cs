using FixedIncome.Application.Factories.Outbox;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Outbox;
using FixedIncome.Infrastructure.Persistence.Outbox;
using Newtonsoft.Json;

namespace FixedIncome.Application.Outbox;

public class OutboxFactory : IOutboxFactory
{

    public OutboxMessage CreateOutboxMessage(EOutboxMessageTypes type, Guid id)
    {
        return type switch
        {
            EOutboxMessageTypes.Email => OutboxMessage.OutboxMessageBuilder(EOutboxMessageTypes.Email.ToString(),
                JsonConvert.SerializeObject(new OutboxEmailMessage
                {
                    Id = id,
                    Email = "defautl@gmail.com",
                    Message = $"Fixed Income with Id {id} was sent to process."
                })),
            EOutboxMessageTypes.File => OutboxMessage.OutboxMessageBuilder(EOutboxMessageTypes.File.ToString(),
                JsonConvert.SerializeObject(new OutboxFileMessage { Id = id })),
            _ => throw new Exception("Invalid type definition")
        };
    }
}