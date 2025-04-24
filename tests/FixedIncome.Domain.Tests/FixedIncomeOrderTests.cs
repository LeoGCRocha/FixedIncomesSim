using FixedIncome.Domain.Common.Enums;
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
        sut.GetEvents.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Have_15_Percent_Tax_When_The_Order_Have_More_Than_3_Years()
    {
        // Arrange & Act 
        var startDate = new DateTime(2024, 03, 03);
        var sut = new FixedIncomeOrder(Guid.NewGuid(), startDate, startDate.AddYears(3), 3000, 1.22m);
        
        // Assert
        sut.Tax.Should().Be(15);
        sut.TaxGroup.Should().Be(ETaxGroup.After720Days);
    }
}