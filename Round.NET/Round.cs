namespace Round.NET;

/*
General design:
    All methods should:
        * Take array of decimals / ints as parameter
        * Take quantity to split by
        * Decimals to round

        * Have two overloads:
            * Returns an array matching the passed in array with the last index contiaining the remainder
            * out parameter for remainder

        * Return an array matching the passed in array with the split values
        * Not modify state

Scenarios to cover:
    * Fixed distribution
    * Proportional distribution
    * Percentage distribution

Things to consider:
    * Midpoint rounding
    * Should remainders be handled in this library or by consumers? 
        - Consumer concern is handled by an out parameter / extra index in array
        - Library concern needs strict rules:
            - Remainder is always added to the last item in the array
            - Remainder is always added to smallest value
            - Remainder is always added to largest value
*/

public enum RoundRemainder
{
    IncludeInResult,
    AddToLastIndex,
    AddToFirstIndex,
    Ignore,
}

public static class Round
{
    public static double[] Fixed(int buckets, double value, int precision, RoundRemainder roundRemainder)
    {
        if (buckets <= 0)
        {
            return new double[2];
        }

        if (buckets == 1)
        {
            return new double[2] { value, 0 };
        }

        // TODO Address midpoint rounding
        var result = new double[buckets + (roundRemainder == RoundRemainder.IncludeInResult ? 1 : 0)];
        var split = Math.Round(value / (double)buckets, precision, MidpointRounding.AwayFromZero);
        for (int i = 0; i < buckets; i++)
        {
            result[i] = split;
        }

        // TODO Address midpoint rounding
        double remainder = Math.Round(value - (split * buckets), precision, MidpointRounding.AwayFromZero);
        switch (roundRemainder)
        {
            case RoundRemainder.IncludeInResult:
                result[buckets] = remainder;
                break;
            case RoundRemainder.AddToFirstIndex:
                result[0] += remainder;
                break;
            case RoundRemainder.AddToLastIndex:
                result[buckets - 1] += remainder;
                break;
            case RoundRemainder.Ignore:
                break;
        }

        return result;
    }

    public static double[] Percentage(double[] percentages, int precision, RoundRemainder remainderType)
    {
        return null;
    }
}
