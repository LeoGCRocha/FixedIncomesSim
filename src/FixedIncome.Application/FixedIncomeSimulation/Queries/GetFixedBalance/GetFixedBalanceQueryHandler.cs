using FixedIncome.Application.Mediator;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;

public class GetFixedBalanceQueryHandler(IFixedIncomeQueryRepository fixedIncomeQueryRepository)
    : IRequestHandler<GetFixedBalanceQuery, IEnumerable<FixedBalanceResponse>>
{
    public async Task<IEnumerable<FixedBalanceResponse>> Handle(GetFixedBalanceQuery request, CancellationToken cancellationToken)
    {
        return await fixedIncomeQueryRepository.GetBalancesById(request.Id);
    }
}