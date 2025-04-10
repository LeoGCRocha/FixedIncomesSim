using FixedIncome.Domain.FixedIncomes;
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
        // Arrange && Act
        var fixedIncome = new FixedIncomeSim(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now.AddMonths(1),
            0,
            1.04m,
            2000);
        
        // Assert
        fixedIncome.FinalAmount.Should().BeApproximately(2020.8m, 0.5m);
        fixedIncome.Profits().Should().BeApproximately(20.8m, 0.5m);
    }
}