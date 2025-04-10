namespace FixedIncome.Domain.Common.Extensions;

public static class DateTimeExtensions
{
    public static int AbsDiffOnDays(this DateTime date, DateTime date2)
    {
        return Math.Abs((date - date2).Days);
    }

}