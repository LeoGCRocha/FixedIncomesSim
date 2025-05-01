using Carter;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FixedIncome.Application.FixedIncomeSimulation;

public class FixedIncomeSimulationModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/simulation", async ([FromBody] CreateFixedIncomeCommand cmd, IMediator mediator) =>
        {
            await mediator.Send(cmd);
            return Results.Ok();
        }).Produces<Created>()
        .WithTags("Simulation");

        app.MapGet("/simulation", async ([AsParameters] GetFixedIncomeQuery query, IMediator mediator) =>
        {
            var response = await mediator.Send(query);
            return response is not null ? Results.Ok(response) : Results.NotFound();
        }).Produces<FixedIncomeResponse>()
        .WithTags("Simulation");
    }
}