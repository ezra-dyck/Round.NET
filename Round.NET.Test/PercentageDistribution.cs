using Xunit;

namespace Round.NET.Test;

public class PercentageDistribution
{
    [Fact]
    public void NullPercentages()
    {
        double[] result = Round.Percentage(null, 0, RoundRemainder.Ignore);
        Assert.Null(result);
    }
}