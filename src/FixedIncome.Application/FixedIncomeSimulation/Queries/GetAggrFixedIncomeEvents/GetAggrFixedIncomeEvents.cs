using System.Globalization;
using Carter;
using CsvHelper;
using CsvHelper.Configuration;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions.Repositories;
using FixedIncome.Application.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            HttpContext context,
            IMediator mediator) =>
        {
            await mediator.Send(new AggrFixedIncomeEventsQuery(id)
            {
                Context = context
            });
        });
    }
}

public class AggrFixedIncomeEventsQuery(Guid fixedIncomeId) : IRequest
{
    public Guid FixedIncomeId { get; init; } = fixedIncomeId;
    public required HttpContext Context { get; set; }
}

public class AggrFixedIncomeEventQueryHandler(IFixedIncomeQueryRepository repository) : IRequestHandler<AggrFixedIncomeEventsQuery>
{
    public async Task Handle(AggrFixedIncomeEventsQuery request, CancellationToken cancellationToken = default)
    {
        // TODO: Change this to can return dateonly instead of datetiem
        // And solve error object disposed
        var response = await repository.GetAggrFixedIncomeEventsById(request.FixedIncomeId);

        request.Context.Response.ContentType = "text/csv";
        
        await using var writer = new StreamWriter(request.Context.Response.Body);
        var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        await csvWriter.WriteRecordsAsync(response, cancellationToken);
        await writer.FlushAsync(cancellationToken);
    }
}