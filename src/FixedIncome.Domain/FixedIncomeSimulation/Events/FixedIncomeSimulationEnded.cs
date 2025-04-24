using MediatR;
using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Domain.FixedIncomeSimulation.Events;

public class FixedIncomeSimulationEnded : DomainEvent, INotification
{
    public FixedIncomeSimulationEnded(Guid id, string type) : base(id, type)
    {
        Id = id;
        Type = type;
    }
}