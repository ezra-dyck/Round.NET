using System;
using System.Linq;
using Xunit;

namespace Round.NET.Test;

public class FixedDistribution
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ZeroOrNegative_IncludeInResult(int buckets)
    {
        double[] result = Round.Fixed(buckets, 1.2D, 2, RoundRemainder.IncludeInResult);

        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal(0, result[0]);
        Assert.Equal(0, result[1]);
    }

    [Theory]
    [InlineData(1, 1.2D, 0)]
    [InlineData(3, 0.4D, 0)]
    [InlineData(4, 0.3D, 0)]
    [InlineData(5, 0.24D, 0)]
    [InlineData(7, 0.17D, 0.01D)]
    public void Positive_IncludeRemainderInResult(int buckets, double expectedBucket, double expectedRemainder)
    {
        double[] result = Round.Fixed(buckets, 1.2D, 2, RoundRemainder.IncludeInResult);

        Assert.NotNull(result);
        Assert.Equal(buckets + 1, result.Length);
        Assert.Equal(expectedRemainder, result[buckets], 2);

        var expected = new double[buckets];
        Array.Fill(expected, expectedBucket);
        Assert.Equal(expected, result.Take(buckets));
    }

    [Fact]
    public void Positive_IgnoreRemainder()
    {
        double[] result = Round.Fixed(7, 1.2D, 2, RoundRemainder.Ignore);

        Assert.NotNull(result);
        Assert.Equal(7, result.Length);

        var expected = new double[7];
        Array.Fill(expected, 0.17D);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Positive_AddRemainderToFirst()
    {
        double[] result = Round.Fixed(7, 1.2D, 2, RoundRemainder.AddToFirstIndex);

        Assert.NotNull(result);
        Assert.Equal(7, result.Length);

        var expected = new double[6];
        Array.Fill(expected, 0.17D);
        Assert.Equal(expected, result.Skip(1).Take(6));
        Assert.Equal(0.18D, result[0], 2);
    }

    [Fact]
    public void Positive_AddRemainderToLast()
    {
        double[] result = Round.Fixed(7, 1.2D, 2, RoundRemainder.AddToLastIndex);

        Assert.NotNull(result);
        Assert.Equal(7, result.Length);

        var expected = new double[6];
        Array.Fill(expected, 0.17D);
        Assert.Equal(expected, result.Take(6));
        Assert.Equal(0.18D, result[6], 2);
    }
}