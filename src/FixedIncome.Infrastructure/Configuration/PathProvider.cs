using FixedIncome.Application.FixedIncomeSimulation.Abstractions;

namespace FixedIncome.Infrastructure.Configuration;

public class PathProvider(PathsConfiguration configuration) : IPathProvider
{
    public string GetOutputDirectory() => configuration.OutputDirectory;
}