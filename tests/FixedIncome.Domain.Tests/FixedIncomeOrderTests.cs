using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using FluentAssertions;

namespace FixedIncome.Domain.Tests;

public class FixedIncomeOrderTests
{
    [Fact]
    public void Should_Create_At_Least_One_Event_If_Range_Is_Less_Than_One_Month()
    {
        // Arrange & Act
        var sut = new FixedIncomeOrder(Guid.NewGuid(), new DateTime(2024, 03, 03), new DateTime(2024, 03, 10), 3000, 1.22m);

        // Assert
        sut.Events.Should().HaveCount(1);
    }
}