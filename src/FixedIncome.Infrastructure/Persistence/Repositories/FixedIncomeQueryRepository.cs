using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetAggrFixedIncomeEvents;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeBalances;

namespace FixedIncome.Infrastructure.Persistence.Repositories;

public class FixedIncomeQueryRepository(IDapperDbContext dapperDbContext) : IFixedIncomeQueryRepository
{
    public async Task<FixedIncomeResponse?> GetResultById(Guid id)
    {
        
        const string query = """
                                 /* 
                                 * [Query: GetFixedIncomeById]
                                 */
                                 SELECT
                                     ROUND(fis."FinalGrossAmount" - fis."InvestedAmount", 2) AS "ProfitAfterTaxes",
                                     ROUND(fis."FinalNetAmount" - fis."InvestedAmount", 2) AS "ProfitBeforeTaxes",
                                     ROUND(fis."InvestedAmount", 2) AS "Amount",
                                     ROUND(fis."FinalGrossAmount", 2) AS "FinalGrossAmount",
                                     ROUND(fis."FinalNetAmount", 2) AS "FinalNetAmount",
                                     fis."StartDate"::date AS "StartDate",
                                     fis."EndDate"::date AS "EndDate",
                                     EXTRACT(DAY FROM (fis."EndDate" - fis."StartDate"))::int AS "TotalInDays"
                                 FROM 
                                     fixed_incomes."fixed_income_simulation" fis
                                 WHERE
                                     fis."Id" = @Id
                             """;
        
         
        var response = await dapperDbContext.GetFirstAsync<FixedIncomeResponse>(query, new
        {
            id
        });

        return response;
    }

    public async Task<IEnumerable<FixedBalanceResponse>> GetBalancesById(Guid id)
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
                             		fib."FixedIncomeSimId" = @Id
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

        var response = await dapperDbContext.GetAsync<FixedBalanceResponse>(query, new
        {
	        id
        });

        return response;
    }

    public async Task<IEnumerable<AggrFixedIncomeEventResponse>> GetAggrFixedIncomeEventsById(Guid id)
    {
	    const string query = """
	                         /*
	                         * [Query: GetAggrFixedIncomeEventsById]
	                         */
	                         SELECT 
	                         count(*) AS "Count",
	                         round(sum(eve."Profit"), 2) AS "Profit",
	                         eve."StartReferenceDate"::date,
	                         eve."EndReferenceDate"::date
	                         FROM 
	                             fixed_incomes.fixed_income_order_event eve
	                         WHERE 
	                             eve."FixedIncomeOrderId" IN (
	                                 SELECT fio."Id"
	                                 FROM fixed_incomes.fixed_income_order fio
	                                 WHERE fio."FixedIncomeSimId" = @Id
	                             )
	                         GROUP BY
	                         	eve."StartReferenceDate"::date,
	                         	eve."EndReferenceDate"::date
	                         ORDER BY eve."StartReferenceDate"::date
	                         """;
	    
	    var response = await dapperDbContext.GetAsync<AggrFixedIncomeEventResponse>(query, new
	    {
			id
	    });

	    return response;
    }
}