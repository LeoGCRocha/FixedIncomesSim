namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;

public class FixedIncomeResponse
{
    public decimal ProfitAfterTaxes { get; set; }
    public decimal ProfitBeforeTaxes { get; set; }
    public decimal Amount { get; set; }
    public decimal FinalNetAmount { get; set; }
    public decimal FinalGrossAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalInDays { get; set; }
}