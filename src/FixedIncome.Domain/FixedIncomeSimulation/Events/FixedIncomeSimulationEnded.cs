using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Domain.FixedIncomeSimulation.Events;

public class FixedIncomeSimulationEnded(Guid id, string type) : DomainEvent(id, type);