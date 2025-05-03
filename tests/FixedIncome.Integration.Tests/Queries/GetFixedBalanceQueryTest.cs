using FluentAssertions;
using FixedIncome.Integration.Tests.Fixture;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;

namespace FixedIncome.Integration.Tests.Queries;

[Collection("FixedIncomeIntegrationTests")]
public class GetFixedBalanceQueryTest
{
    private readonly FixedIncomeFixture _fixture = new();

    [Fact(DisplayName = "Should return current aging to all months")]
    public async Task Should_Return_Aging_To_All_Months()
    {
        // Arrange
        var startDate = new DateTime(2024, 04, 04);
        var fixedIncomeSim = new FixedIncomeSim(Guid.NewGuid(), startDate, startDate.AddYears(1), 40000, 1.22m, 1.17m);
        await _fixture.CreateFixedIncome(fixedIncomeSim);
        
        // Act
        var result = await _fixture.SendCommand(new GetFixedBalanceQuery { Id = fixedIncomeSim.Id }) as IEnumerable<FixedBalanceResponse>;

        // Assert
        result.Should().HaveCount(12);
    }
}