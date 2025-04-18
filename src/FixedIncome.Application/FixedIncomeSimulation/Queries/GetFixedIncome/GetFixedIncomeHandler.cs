using MediatR;
using FixedIncome.Infrastructure.Persistence.Abstractions;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;

public class GetFixedIncomeHandler : IRequestHandler<GetFixedIncomeQuery, FixedIncomeResponse>
{
    private readonly IDapperDbContext _dapperDbContext;

    public GetFixedIncomeHandler(IDapperDbContext dbContext)
    {
        _dapperDbContext = dbContext;
    }
    
    public async Task<FixedIncomeResponse> Handle(GetFixedIncomeQuery request, CancellationToken cancellationToken)
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

        var response = await _dapperDbContext.ExecuteAsync<FixedIncomeResponse>(query, new
        {
			request.Id
        });

        return response ?? new FixedIncomeResponse();
    }
}