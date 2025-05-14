namespace FixedIncome.Application.Helper;

public sealed class FixedIncomeSimulationLinq2Db
{
    public Guid Id { get; set; }
    public string InvestmentTitle { get; set; } = string.Empty;
    public int InformationType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal StartAmount { get; set; }
    public decimal MonthlyYield { get; set; }
    public decimal MonthlyContribution { get; set; }
    public decimal InvestedAmount { get; set; }
    public decimal FinalGrossAmount { get; set; }
    public decimal FinalNetAmount { get; set; }
}

public sealed class FixedIncomeOrderLinq2Db
{
    public Guid Id { get; set; }
    public Guid FixedIncomeSimId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal StartAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public decimal Tax { get; set; }
    public int TaxGroup { get; set; }
    public decimal MonthlyYield { get; set; }
}

public sealed class FixedIncomeEventLinq2Db
{
    public Guid Id { get; set; }
    public Guid FixedIncomeOrderId { get; set; }
    public DateTime StartReferenceDate { get; set; }
    public DateTime EndReferenceDate { get; set; }
    public decimal StartAmount { get; set; }
    public decimal Amount { get; set; }
    public decimal Profit { get; set; }
    public decimal MonthlyYield { get; set; }
}

public sealed class FixedIncomeBalanceLinq2Db
{
    public Guid Id { get; set; }
    public Guid FixedIncomeSimId { get; set; }
    public DateTime ReferenceDate { get; set; }
    public decimal Amount { get; set; }
    public decimal Profit { get; set; }
}