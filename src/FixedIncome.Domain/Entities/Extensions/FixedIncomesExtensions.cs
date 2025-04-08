namespace FixedIncome.Domain.FixedIncomes.Extensions;

public static class FixedIncomesExtensions
{
    public static decimal ToDailyYield(this decimal monthlyYield)
    {
        double baseValue = (double)(1 + monthlyYield / 100);
        double dailyRate = Math.Pow(baseValue, 1.0 / 30) - 1;
        return (decimal)dailyRate;
    }
}