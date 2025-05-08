using MediatR;
using CsvHelper;
using System.Globalization;
using FixedIncome.Application.FixedIncomeSimulation.Abstractions;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;

public class CreateBalanceFileHandler : IRequestHandler<CreateBalanceFileCommand>
{
    private readonly IMediator _mediator;
    private readonly IPathProvider _pathsConfiguration;

    public CreateBalanceFileHandler(IMediator mediator, IPathProvider configuration)
    {
        _mediator = mediator;
        _pathsConfiguration = configuration;
    }

    public async Task Handle(CreateBalanceFileCommand request, CancellationToken cancellationToken)
    {
        var command = new GetFixedBalanceQuery { Id = request.Id };
        var response = await _mediator.Send(command, cancellationToken);

        var outputDir = _pathsConfiguration.GetOutputDirectory();
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        
        await using var writer = new StreamWriter(Path.Combine(outputDir, $"{request.Id}.csv"));
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(response, cancellationToken);
    }
}