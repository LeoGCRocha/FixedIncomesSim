using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using MediatR;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;

public class GetFixedIncomeQueryHandler : IRequestHandler<GetFixedIncomeQuery, FixedIncomeResponse?>
{
    private readonly IFixedIncomeQueryRepository _fixedIncomeQueryRepository;

    public GetFixedIncomeQueryHandler(IFixedIncomeQueryRepository fixedIncomeQueryRepository)
    {
        _fixedIncomeQueryRepository = fixedIncomeQueryRepository;
    }
    
    public async Task<FixedIncomeResponse?> Handle(GetFixedIncomeQuery request, CancellationToken cancellationToken)
    {
        return await _fixedIncomeQueryRepository.GetResultById(request.Id);
    }
}