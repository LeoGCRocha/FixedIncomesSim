using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.Common.Enums;
using FixedIncome.Domain.Common.Extensions;
using FixedIncome.Domain.FixedIncomes.Extensions;

namespace FixedIncome.Domain.FixedIncomes.FixedIncomeOrders;

public class FixedIncomeOrder : Entity<Guid>
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal StartAmount { get; private set; }
    public decimal CurrentAmount { get; private set; }
    public decimal Tax { get; private set; }
    public ETaxGroup TaxGroup { get; private set; }
    public decimal MonthlyYield { get; private set; }
    
    private readonly List<FixedIncomeOrderEvent> _events = [];
    public FixedIncomeOrder(Guid id, 
        DateTime startDate, 
        DateTime endDate, 
        decimal startAmount,
        decimal monthlyYield) : base(id)
    {
        StartDate = startDate;
        EndDate = endDate;
        StartAmount = startAmount;
        CurrentAmount = StartAmount;
        MonthlyYield = monthlyYield;
        TaxGroup = GetTaxGroupFromDatesDiff();
        Tax = GetTaxFromDatesDiff();

        GenerateEvents();
    }

    private ETaxGroup GetTaxGroupFromDatesDiff()
    {
        var daysDiff = StartDate.AbsDiffOnDays(EndDate);
        
        return daysDiff switch
        {
            <= 180 => ETaxGroup.Before180Days,
            <= 360 => ETaxGroup.Before360Days,
            <= 720 => ETaxGroup.Before720Days,
            _ => ETaxGroup.After720Days
        };
    }
    
    private decimal GetTaxFromDatesDiff()
    {
        var daysDiff = StartDate.AbsDiffOnDays(EndDate);
        
        return daysDiff switch
        {
            <= 180 => 22.5m,
            <= 360 => 20.5m,
            <= 720 => 17.5m,
            _ => 15
        };
    }

    private void GenerateEvents()
    {
        var currentDate = StartDate;
        while (currentDate < EndDate)
        {
            var nextMonth = currentDate.AddMonths(1);
            if (nextMonth > EndDate)
                nextMonth = EndDate;

            var orderEvent = new FixedIncomeOrderEvent(Guid.NewGuid(), currentDate, nextMonth, CurrentAmount, MonthlyYield);
            _events.Add(orderEvent);
            CurrentAmount += orderEvent.Profit;
            currentDate = nextMonth;
        }
    }

    internal decimal NetProfit()
    {
        decimal allTimeProfit = _events.Where(e => e.EndReferenceDate <= EndDate).Sum(e => e.Profit);
        return  allTimeProfit - allTimeProfit * (Tax / 100);
    }
    
    internal decimal ProfitUntilPeriod(DateTime endDate)
    {
        return _events.Where(e => e.EndReferenceDate <= endDate).Sum(e => e.Profit);
    }
}