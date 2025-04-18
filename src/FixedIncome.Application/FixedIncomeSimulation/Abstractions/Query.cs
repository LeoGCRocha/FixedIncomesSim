namespace FixedIncome.Application.FixedIncomeSimulation.Abstractions;

public class Query<TResponse> : IQuery<TResponse>
{
    public Guid Id { get; set; }

    protected Query()
    {
        Id = Guid.NewGuid();
    }
}