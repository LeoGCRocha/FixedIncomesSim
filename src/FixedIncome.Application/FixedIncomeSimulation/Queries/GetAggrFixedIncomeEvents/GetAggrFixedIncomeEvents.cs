using Carter;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using FixedIncome.Application.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetAggrFixedIncomeEvents;

// TODO: Add docs at here
public class GetAggrFixedIncomeEvents : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/aggr-fixed-income-events/{id}", async (
            [FromQuery] Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(new AggrFixedIncomeEventsQuery(id));
        });
    }
}

public class AggrFixedIncomeEventsQuery(Guid fixedIncomeId) : IRequest
{
    public Guid FixedIncomeId { get; init; } = fixedIncomeId;
}

public class AggrFixedIncomeEventQueryHandler(IFixedIncomeQueryRepository repository) : IRequestHandler<AggrFixedIncomeEventsQuery>
{
    public async Task Handle(AggrFixedIncomeEventsQuery request, CancellationToken cancellationToken = default)
    {
        var response = await repository.GetAggrFixedIncomeEventsById(request.FixedIncomeId);
        // TODO: continue here;
    }
}