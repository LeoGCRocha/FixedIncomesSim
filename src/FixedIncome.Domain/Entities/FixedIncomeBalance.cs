using FixedIncome.Domain.Abstractions;

namespace FixedIncome.Domain.Entities;

public class FixedIncomeBalance : Entity<Guid>
{
    public Guid FixedIncomeId { get; init; }
    public DateTime ReferenceDate { get; init; }
    public decimal Amount { get; private set; }
    public decimal Profit { get; private set;  }
    public FixedIncomeBalance(Guid id, Guid fixedIncomeId, DateTime referenceDate, decimal amount, decimal profit) : base(id)
    {
        FixedIncomeId = fixedIncomeId;
        ReferenceDate = referenceDate;
        Amount = amount;
        Profit = profit;
    }
    public void AddAmount(decimal amount) => Amount += amount;
    public void AddProfit(decimal profit) => Profit += profit;
}