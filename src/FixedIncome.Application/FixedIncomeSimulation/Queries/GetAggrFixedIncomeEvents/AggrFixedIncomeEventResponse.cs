namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetAggrFixedIncomeEvents;

public class AggrFixedIncomeEventResponse
{
    public int Count { get; set; }
    public decimal Profit { get; set; }
    public DateOnly StartReferenceDate { get; set; }
    public DateOnly EndReferenceDate { get; set; }
}