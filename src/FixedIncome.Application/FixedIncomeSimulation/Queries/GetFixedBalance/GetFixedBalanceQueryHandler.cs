using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using MediatR;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;

public class GetFixedBalanceQueryHandler : IRequestHandler<GetFixedBalanceQuery, IEnumerable<FixedBalanceResponse>>
{
    private IFixedIncomeQueryRepository _fixedIncomeQueryRepository;
    
    public GetFixedBalanceQueryHandler(IFixedIncomeQueryRepository fixedIncomeQueryRepository)
    {
        _fixedIncomeQueryRepository = fixedIncomeQueryRepository;
    }

    public async Task<IEnumerable<FixedBalanceResponse>> Handle(GetFixedBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _fixedIncomeQueryRepository.GetBalancesById(request.Id);
    }
}