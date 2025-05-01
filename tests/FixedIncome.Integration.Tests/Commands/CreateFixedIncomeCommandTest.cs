using Bogus;
using FixedIncome.Integration.Tests.Fixture;

namespace FixedIncome.Integration.Tests.Commands;

public class CreateFixedIncomeCommandTest
{
    private readonly FixedIncomeFixture _fixture = new();
    private readonly Faker _faker = new Faker();

    [Fact]
    public async Task Should_Return_Fixed_Income_Id_After_Insert()
    {
        // Arrange
        // var fixedIncome = new FixedIncomeSim(
        //     Guid.NewGuid(),
        //     request.StartDate,
        //     request.EndDate,
        //     request.StartAmount,
        //     request.MonthlyYield,
        //     request.MonthlyContribution
        // );
        
        // public class CreateFixedIncomeCommand : IRequest<CreateFixedIncomeCommandResponse>
        // {
        //     public DateTime StartDate { get; set; }
        //     public DateTime EndDate { get; set; }
        //     public decimal StartAmount { get; set; }
        //     public decimal MonthlyYield { get; set; }
        //     public decimal MonthlyContribution { get; set; }
        //     public InvestmentInformation? Information { get; set; }
        // }
        
        // Act
        
        // Assert
    }
}