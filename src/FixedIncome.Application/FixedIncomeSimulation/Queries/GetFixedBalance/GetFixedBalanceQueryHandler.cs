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
                          WITH BalanceResult AS (
                          	SELECT
                          	  	fib."ReferenceDate"::date AS "ReferenceDate",
                          		fib."Amount" AS "Amount",
                          		fib."Profit" AS "Profit",
                          		LAG(fib."Amount", 1, 0) OVER (ORDER BY fib."ReferenceDate") AS "LagAmount"
                          	FROM
                          	  	fixed_incomes.fixed_income_balance fib
                          	WHERE
                          		fib."FixedIncomeId" = @Id
                          	ORDER BY
                          		fib."ReferenceDate"
                          )
                          SELECT
                          	br."ReferenceDate" ,
                          	br."Amount",
                          	br."Profit",
                          	CASE
                          		WHEN br."LagAmount" = 0 THEN 0
                          		ELSE br."Amount" - br."LagAmount"
                          	END AS "MonthlyVariation"
                          FROM
                          	BalanceResult br
                          """;
         
         var response = await _dapperDbContext.GetAsync<FixedBalanceResponse>(query, new
         {
             request.Id
         });
 
         return response;
     }
}