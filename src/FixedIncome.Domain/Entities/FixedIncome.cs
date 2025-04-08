using FixedIncome.Domain.Abstractions;
using FixedIncome.Domain.FixedIncomes.Events;

namespace FixedIncome.Domain.Entities;

public class FixedIncome : AggregateRoot<Guid>
{
    private readonly List<FixedIncomeOrder> _orders = [];
    private readonly List<FixedIncomeBalance> _balances = [];
    
    public FixedIncome(
        Guid id, 
        DateTime startDate, 
        DateTime endDate,
        decimal startAmount, 
        decimal monthlyYield,
        decimal monthlyContribution
        ) : base(id)
    {
        if (startDate > endDate)
            throw new Exception("Invalid definition, startDate cannot be greater than endDate.");
        
        StartDate = startDate;
        StartAmount = startAmount;
        MonthlyYield = monthlyYield;
        MonthlyContribution = monthlyContribution;
        EndDate = endDate;
        InvestedAmount = StartAmount;
        
        Simulate();
        GenerateBalance();
    }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    private decimal StartAmount { get; }
    private decimal MonthlyYield { get; }
    public decimal MonthlyContribution { get; private set; }
    public decimal InvestedAmount { get; private set; }
    public decimal FinalAmount { get; private set; }
    public decimal FinalAmountNet { get; private set; }
    
    private readonly Dictionary<DateTime, decimal> _monthlyProfits = new ();
    private readonly Dictionary<DateTime, decimal> _monthlyNetProfits = new ();
    
    private void Simulate()
    {
        var currentDate = StartDate;
        while (currentDate.AddMonths(1) < EndDate)
        {
            InvestedAmount += MonthlyContribution;
            var orderToAdd = new FixedIncomeOrder(Guid.NewGuid(), Id, currentDate, EndDate, MonthlyContribution,
                MonthlyYield);
            _orders.Add(orderToAdd);
            
            _monthlyProfits[currentDate.Date] = orderToAdd.ProfitUntilPeriod(EndDate);
            _monthlyNetProfits[currentDate.Date] = orderToAdd.NetProfit();
            
            currentDate = currentDate.AddMonths(1);
        }

        FinalAmount = InvestedAmount + _monthlyProfits.Sum(e => e.Value);
        FinalAmountNet = InvestedAmount + _monthlyNetProfits.Sum(e => e.Value);
        
        RaiseDomainEvent(new FixedIncomeSimulationEnded(Id, nameof(FixedIncome)));
    }

    private void GenerateBalance()
    {
        var currentDate = StartDate;
        int count = 1;
        while (currentDate.AddMonths(1) < EndDate)
        {
            var balance = new FixedIncomeBalance(Guid.NewGuid(), Id, currentDate.AddMonths(1), 0, 0);

            balance.AddProfit(_monthlyNetProfits[currentDate.Date]);
            balance.AddAmount(count * MonthlyContribution + balance.Profit);
            
            _balances.Add(balance);
            
            currentDate = currentDate.AddMonths(1);
            count++;
        }
    }

    public IReadOnlyList<FixedIncomeBalance> GetBalancesHistory()
    {
        return _balances.AsReadOnly();
    }

    public IReadOnlyList<FixedIncomeOrder> GetOrdersHistory()
    {
        return _orders.AsReadOnly();
    }

    public decimal Profits()
    {
        return _monthlyProfits.Sum(e => e.Value);
    }
}