using System.Globalization;
using Carter;
using CsvHelper;
using CsvHelper.Configuration;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using FixedIncome.Application.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalanceFile;

public class GetFixedBalanceFileModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("balance-file/{fixedIncomeId}", async (
            [FromQuery] Guid fixedIncomeId,
            IMediator mediator) =>
        {
            var response = await mediator.Send(new BalanceFileQuery(fixedIncomeId));

            return Results.File(
                response,
                contentType: "text/csv");
        });
    }
}

public class BalanceFileQuery(Guid fixedIncomeId) : IRequest<MemoryStream>
{
    public Guid FixedIncomeId { get; } = fixedIncomeId;
}

public class BalanceFileQueryHandler(IMediator mediator) : IRequestHandler<BalanceFileQuery, MemoryStream>
{
    public async Task<MemoryStream> Handle(BalanceFileQuery request, CancellationToken cancellationToken)
    {
        // To small files Results.File can be used
        var command = new GetFixedBalanceQuery { Id = request.FixedIncomeId };
        var response = await mediator.Send(command, cancellationToken);

        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);
        var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
        
        await csvWriter.WriteRecordsAsync(response, cancellationToken);
        
        await streamWriter.FlushAsync(cancellationToken);
        memoryStream.Position = 0;
        
        return memoryStream;
    }
}