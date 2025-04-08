using FixedIncome.Domain.Abstractions;
using FixedIncome.Domain.FixedIncomes.Extensions;

namespace FixedIncome.Domain.Entities;

public class FixedIncomeOrderEvent : Entity<Guid>
{
    public DateTime StartReferenceDate { get; }
    public DateTime EndReferenceDate { get; }
    private decimal StartAmount { get; }
    private decimal MonthlyYield { get; }
    private Guid FixedIncomeOrderId { get; }
    public decimal Amount { get; private set; }
    public decimal Profit { get; private set; }

    public FixedIncomeOrderEvent(Guid id, 
        Guid fixedIncomeOrderId, 
        DateTime startReferenceDate,
        DateTime endReferenceDate, 
        decimal amount, 
        decimal monthlyYield) : base(id)
    {
        StartReferenceDate = startReferenceDate;
        EndReferenceDate = endReferenceDate;
        StartAmount = amount;
        MonthlyYield = monthlyYield;
        FixedIncomeOrderId = fixedIncomeOrderId;

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