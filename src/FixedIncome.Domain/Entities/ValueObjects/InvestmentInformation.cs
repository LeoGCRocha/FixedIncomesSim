using FixedIncome.Domain.FixedIncomes.Shared;
using FixedIncome.Domain.Primitives;

namespace FixedIncome.Domain.FixedIncomes.ValueObjects;

public class InvestmentInformation : ValueObject
{
    public required string Title { get; set; }
    public EFixedIncomeOrderType Type { get; private set; }
    
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