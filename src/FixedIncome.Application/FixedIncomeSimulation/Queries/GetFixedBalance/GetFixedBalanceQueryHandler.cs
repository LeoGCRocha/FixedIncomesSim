using MediatR;
using FixedIncome.Infrastructure.Persistence.Abstractions;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;

public class GetFixedBalanceQueryHandler : IRequestHandler<GetFixedBalanceQuery, IEnumerable<FixedBalanceResponse>>
{
    private readonly IDapperDbContext _dapperDbContext;
    public GetFixedBalanceQueryHandler(IDapperDbContext dapperDbContext)
    {
        _dapperDbContext = dapperDbContext;
    }

    public async Task<IEnumerable<FixedBalanceResponse>> Handle(GetFixedBalanceQuery request, CancellationToken cancellationToken)
     {
         const string query = """
                              /* 
                              * [Query: GetBalancesByFixedIncomeId]
                              */
                              SELECT
                              	fib."ReferenceDate"::date AS "ReferenceDate",
                              	fib."Amount" AS "Amount",
                              	fib."Profit" AS "Profit" 
                              FROM
                              	fixed_incomes.fixed_income_balance fib
                              WHERE fib."FixedIncomeId" = @Id
                              ORDER BY fib."ReferenceDate"
                          """;
         
         var response = await _dapperDbContext.GetAsync<FixedBalanceResponse>(query, new
         {
             request.Id
         });
 
         return response;
     }
}