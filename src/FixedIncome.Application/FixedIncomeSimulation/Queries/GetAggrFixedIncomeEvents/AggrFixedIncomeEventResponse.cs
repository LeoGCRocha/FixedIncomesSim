namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetAggrFixedIncomeEvents;

public class AggrFixedIncomeEventResponse
{
    public int Count { get; set; }
    public decimal Profit { get; set; }
    public DateTime StartReferenceDate { get; set; }
    public DateTime EndReferenceDate { get; set; }
}