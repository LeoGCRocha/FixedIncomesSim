using FixedIncome.Domain.FixedIncomeSimulation;
using FluentAssertions;

namespace FixedIncome.Domain.Tests;

public class FixedIncomeSimTest
{
    [Fact]
    public void Should_Create_12_Orders_When_A_Year_FixedIncome_Is_Created()
    {
        // Arrange && Act
        var fixedIncome = new FixedIncomeSim(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now.AddMonths(12),
            2000,
            1.04m,
            20000);

        // Assert
        fixedIncome.GetOrdersHistory().Count.Should().Be(12);
    }

    [Fact]
    public void Should_Simulate_Correct_Profit_To_Fixed_Income()
    {
        // Arrange & Act
        var fixedIncome = new FixedIncomeSim(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddMonths(12), 44590.54m, 1.04m, 2500);

        fixedIncome.GetOrdersHistory().Count.Should().Be(13);
    }
}