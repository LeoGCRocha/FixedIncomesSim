using CsvHelper;
using System.Globalization;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using FixedIncome.Application.Mediator;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;

public class CreateBalanceFileHandler(IMediator mediator, IPathProvider configuration)
    : IRequestHandler<CreateBalanceFileCommand>
{
    public async Task Handle(CreateBalanceFileCommand request, CancellationToken cancellationToken)
    {
        var command = new GetFixedBalanceQuery { Id = request.Id };
        var response = await mediator.Send(command, cancellationToken);
        
        var outputDir = configuration.GetOutputDirectory();
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        
        await using var writer = new StreamWriter(Path.Combine(outputDir, $"{request.Id}.csv"));
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(response, cancellationToken);
    }
}