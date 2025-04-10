using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Domain.FixedIncomes.Events;

public class FixedIncomeSimulationEnded(Guid id, string type) : DomainEvent(id, type);