using FixedIncome.Domain.Abstractions;

namespace FixedIncome.Domain.Entities;

public class FixedIncomeBalance : Entity<Guid>
{
    public DateTime ReferenceDate { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Profit { get; private set;  }
    public FixedIncomeBalance(Guid id, DateTime referenceDate, decimal amount, decimal profit) : base(id)
    {
        ReferenceDate = referenceDate;
        Amount = amount;
        Profit = profit;
    }
    public void AddAmount(decimal amount) => Amount += amount;
    public void AddProfit(decimal profit) => Profit += profit;
}