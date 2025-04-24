using FixedIncome.Integration.Tests.Fixture;

namespace FixedIncome.Integration.Tests;

public class CreateFixedIncomeCommandTest
{
    private readonly FixedIncomeFixture _fixture;

    public CreateFixedIncomeCommandTest()
    {
        _fixture = new FixedIncomeFixture();
    }

    [Fact]
    public void Should_1_1()
    {
        Assert.Equal(1,1 );
    }
}