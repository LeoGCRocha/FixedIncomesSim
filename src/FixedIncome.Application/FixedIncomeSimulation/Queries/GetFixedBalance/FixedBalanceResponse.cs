namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;

public class FixedBalanceResponse
{
    public DateTime ReferenceDate { get; set; }
    public decimal Amount { get; set; }
    public decimal Profit { get; set; }
}