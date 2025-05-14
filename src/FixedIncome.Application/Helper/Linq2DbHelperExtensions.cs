using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;

namespace FixedIncome.Application.Helper;

public static class Linq2DbHelperExtensions
{
    public static FixedIncomeOrderLinq2Db FixedIncomeOrderToLinq(this FixedIncomeOrder order, Guid id)
    {
        return new FixedIncomeOrderLinq2Db 
        {
            FixedIncomeSimId = id,
            Id = order.Id,
            StartDate = order.StartDate,
            EndDate = order.EndDate,
            StartAmount = order.StartAmount,
            CurrentAmount = order.CurrentAmount,
            Tax = order.Tax,
            TaxGroup = (int) order.TaxGroup,
            MonthlyYield = order.MonthlyYield
        };
    }

    public static FixedIncomeSimulationLinq2Db FixedIncomeSimulationToLinq(this FixedIncomeSim fixedIncomeSim)
    {
        return new FixedIncomeSimulationLinq2Db
        {
            InvestmentTitle = fixedIncomeSim.Information.Title,
            InformationType = (int) fixedIncomeSim.Information.Type,
            Id = fixedIncomeSim.Id,
            StartDate = fixedIncomeSim.StartDate,
            EndDate = fixedIncomeSim.EndDate,
            StartAmount = fixedIncomeSim.StartAmount,
            MonthlyYield = fixedIncomeSim.MonthlyYield,
            MonthlyContribution = fixedIncomeSim.MonthlyContribution,
            InvestedAmount = fixedIncomeSim.InvestedAmount,
            FinalGrossAmount = fixedIncomeSim.FinalGrossAmount,
            FinalNetAmount = fixedIncomeSim.FinalNetAmount
        };
    }

    public static FixedIncomeEventLinq2Db OrderEventToLinq(this FixedIncomeOrderEvent orderEvent, Guid orderId)
    {
        return new FixedIncomeEventLinq2Db
        {
            Id = orderEvent.Id,
            FixedIncomeOrderId = orderId,
            StartReferenceDate = orderEvent.StartReferenceDate,
            StartAmount = orderEvent.StartAmount,
            EndReferenceDate = orderEvent.EndReferenceDate,
            MonthlyYield = orderEvent.MonthlyYield,
            Amount = orderEvent.Amount,
            Profit = orderEvent.Profit
        };
    }

    public static FixedIncomeBalanceLinq2Db BalanceToLinq(this FixedIncomeBalance balance, Guid id)
    {
        return new FixedIncomeBalanceLinq2Db
        {
            Id = balance.Id,
            FixedIncomeSimId = id,
            ReferenceDate = balance.ReferenceDate,
            Profit = balance.Profit,
            Amount = balance.Amount
        };
    }
}