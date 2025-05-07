using Carter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;

namespace FixedIncome.Application.FixedIncomeSimulation;

public class CreateFixedIncomeBalanceFileModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("balance-file", async ([AsParameters] CreateBalanceFileCommand cmd, IMediator mediator) =>
        {
            await mediator.Send(cmd);
            return Results.Ok();
        });
    }
}