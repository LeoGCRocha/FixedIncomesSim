using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.Common.Enums;
using FixedIncome.Domain.Common.ValueObjects;
using FixedIncome.Domain.FixedIncomeSimulation.Events;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;

namespace FixedIncome.Domain.FixedIncomeSimulation;

public sealed class FixedIncomeSim : AggregateRoot<Guid>
{
    private readonly List<FixedIncomeOrder> _orders = [];
    private readonly List<FixedIncomeBalance> _balances = [];

    public FixedIncomeSim(
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

    public void SetInformation(string title, EFixedIncomeOrderType type)
    {
        Information.Title = title;
        Information.Type = type;
    }
    
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal StartAmount { get; }
    public decimal MonthlyYield { get; }
    public decimal MonthlyContribution { get; private set; }
    public decimal InvestedAmount { get; private set; }
    public decimal FinalAmount { get; private set; }
    public decimal FinalAmountNet { get; private set; }
    public InvestmentInformation Information { get; private set; }
    
    private readonly Dictionary<DateTime, decimal> _monthlyProfits = new ();
    private readonly Dictionary<DateTime, decimal> _monthlyNetProfits = new ();
    
    private void Simulate()
    {
        var currentDate = StartDate;
        while (currentDate.AddMonths(1) < EndDate)
        {
            InvestedAmount += MonthlyContribution;
            var orderToAdd = new FixedIncomeOrder(Guid.NewGuid(), currentDate, EndDate, MonthlyContribution,
                MonthlyYield);
            _orders.Add(orderToAdd);
            
            _monthlyProfits[currentDate.Date] = orderToAdd.ProfitUntilPeriod(EndDate);
            _monthlyNetProfits[currentDate.Date] = orderToAdd.NetProfit();
            
            currentDate = currentDate.AddMonths(1);
        }

        FinalAmount = InvestedAmount + _monthlyProfits.Sum(e => e.Value);
        FinalAmountNet = InvestedAmount + _monthlyNetProfits.Sum(e => e.Value);
        
        RaiseDomainEvent(new FixedIncomeSimulationEnded(Id, nameof(FixedIncomeSim)));
    }

    private void GenerateBalance()
    {
        var currentDate = StartDate;
        int count = 1;
        while (currentDate.AddMonths(1) < EndDate)
        {
            var balance = new FixedIncomeBalance(Guid.NewGuid(), currentDate.AddMonths(1), 0, 0);

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