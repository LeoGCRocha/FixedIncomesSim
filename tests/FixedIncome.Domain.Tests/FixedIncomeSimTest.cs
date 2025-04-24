using FixedIncome.Domain.FixedIncomeSimulation;
using FluentAssertions;

namespace FixedIncome.Domain.Tests;

public class FixedIncomeSimTest
{
    
    [Fact]
    public void Should_Create_1_Order_If_Range_Is_Less_Than_1_Month()
    {
        // Arrange & Act
        var sut = new FixedIncomeSim(Guid.NewGuid(), new DateTime(2024, 03, 03), new DateTime(2024, 03, 10), 10000, 1.22m, 3000);

        // Assert
        sut.Orders.Should().HaveCount(1);
    }

}