using FixedIncome.Application.Mediator;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;

public class GetFixedIncomeQueryHandler(IFixedIncomeQueryRepository fixedIncomeQueryRepository) 
    : IRequestHandler<GetFixedIncomeQuery, FixedIncomeResponse?>
{
    public async Task<FixedIncomeResponse?> Handle(GetFixedIncomeQuery request, CancellationToken cancellationToken)
    {
        return await fixedIncomeQueryRepository.GetResultById(request.Id);
    }
}