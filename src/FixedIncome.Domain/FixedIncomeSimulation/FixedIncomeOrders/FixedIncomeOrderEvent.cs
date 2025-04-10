using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.Common.Extensions;
using FixedIncome.Domain.FixedIncomeSimulation.Extensions;

namespace FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;

public class FixedIncomeOrderEvent : Entity<Guid>
{
    public DateTime StartReferenceDate { get; }
    public DateTime EndReferenceDate { get; }
    public decimal StartAmount { get; private set;  }
    public decimal MonthlyYield { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Profit { get; private set; }

    public FixedIncomeOrderEvent(Guid id, 
        DateTime startReferenceDate,
        DateTime endReferenceDate, 
        decimal amount, 
        decimal monthlyYield) : base(id)
    {
        StartReferenceDate = startReferenceDate;
        EndReferenceDate = endReferenceDate;
        StartAmount = amount;
        MonthlyYield = monthlyYield;

        Simulate();
    }
    private void Simulate()
    {
        var daysDiff = StartReferenceDate.AbsDiffOnDays(EndReferenceDate);
        var dailyYield = MonthlyYield.ToDailyYield();
        Profit = StartAmount * dailyYield  * daysDiff;
        Amount = StartAmount + Profit;
    }
}