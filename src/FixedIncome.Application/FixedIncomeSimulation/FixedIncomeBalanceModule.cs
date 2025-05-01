using Carter;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FixedIncome.Application.FixedIncomeSimulation;

public class FixedIncomeBalanceModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("balance", async ([AsParameters] GetFixedBalanceQuery query, IMediator mediator) =>
        {
            var response = await mediator.Send(query);
            return Results.Ok(response);
        });
    }
}