using Carter;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace FixedIncome.Application.FixedIncomeSimulation;

public class FixedIncomeSimulationModule : ICarterModule
{
    private readonly IMediator _mediator;

    public FixedIncomeSimulationModule(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost<CreateFixedIncomeCommand>("/simulation", async (CreateFixedIncomeCommand cmd) =>
        {
            await _mediator.Send(cmd);
        });
    }
}