using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.Common.Enums;

namespace FixedIncome.Domain.Common.ValueObjects;

public class InvestmentInformation : ValueObject
{
    public string Title { get; set; }
    public EFixedIncomeOrderType Type { get;  set; }
    
    public InvestmentInformation(string title, EFixedIncomeOrderType type)
    {
        Title = title;
        Type = type;
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Title;
        yield return Type;
    }
}