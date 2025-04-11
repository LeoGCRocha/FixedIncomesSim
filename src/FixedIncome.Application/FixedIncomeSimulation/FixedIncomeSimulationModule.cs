using Carter;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FixedIncome.Application.FixedIncomeSimulation;

public class FixedIncomeSimulationModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/simulation", async (CreateFixedIncomeCommand cmd, IMediator mediator) =>
        {
            await mediator.Send(cmd);
            return Results.Ok();
        }).Produces<Created>()
        .WithTags("Simulation");
    }
}