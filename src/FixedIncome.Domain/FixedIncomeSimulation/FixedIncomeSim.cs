using FixedIncome.Domain.Common.Enums;
using FixedIncome.Domain.Common.Abstractions;
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
        Information = new InvestmentInformation(title, type);
    }
    
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal StartAmount { get; }
    public decimal MonthlyYield { get; }
    public decimal MonthlyContribution { get; private set; }
    public decimal InvestedAmount { get; private set; }
    public decimal FinalGrossAmount { get; private set; }
    public decimal FinalNetAmount { get; private set; }
    public InvestmentInformation Information { get; private set; }
    
    private readonly Dictionary<DateTime, decimal> _monthlyProfits = new ();

    public IReadOnlyList<FixedIncomeOrder> GetOrders => _orders.AsReadOnly();
    
    private void Simulate()
    {
        var currentDate = StartDate;

        while (currentDate < EndDate)
        {
            FixedIncomeOrder order;
            if (currentDate == StartDate)
            {
                order = new FixedIncomeOrder(Guid.NewGuid(), currentDate, EndDate, StartAmount + MonthlyContribution, MonthlyYield);
            }
            else
            {
                order = new FixedIncomeOrder(Guid.NewGuid(), currentDate, EndDate, MonthlyContribution,
                    MonthlyYield);
            }
            
            InvestedAmount += MonthlyContribution;
            _orders.Add(order);
            
            currentDate = currentDate.AddMonths(1);
        }
        
        currentDate = StartDate;
        while (currentDate < EndDate)
        {
            foreach (var order in _orders)
            {
                var profitPeriod = order.PeriodProfit(currentDate.Date, currentDate.Date.AddMonths(1));
                if (!_monthlyProfits.TryAdd(currentDate.Date, profitPeriod))
                    _monthlyProfits[currentDate.Date] += profitPeriod;
            }
        
            currentDate = currentDate.AddMonths(1);
        }

        FinalGrossAmount = InvestedAmount + _monthlyProfits.Sum(e => e.Value);
        FinalNetAmount = InvestedAmount + _orders.Sum(e => e.NetProfit());
        
        RaiseDomainEvent(new FixedIncomeSimulationEnded(Id, nameof(FixedIncomeSim)));
    }

    private void GenerateBalance()
    {
        int count = 1;
        var currentDate = StartDate;
        decimal currentProfit = 0;
        while (currentDate < EndDate)
        {
            var balance = new FixedIncomeBalance(Guid.NewGuid(), currentDate.AddMonths(1), 0, 0);

            currentProfit += _monthlyProfits[currentDate.Date];

            balance.SetProfit(currentProfit);
            balance.AddAmount(StartAmount + count * MonthlyContribution + balance.Profit);
            
            _balances.Add(balance);
            
            currentDate = currentDate.AddMonths(1);
            count++;
        }
    }
}