namespace Solinor.MonthlyWageCalculation.Extensions
{
    /// <summary>
    /// Helper extension for double to do double to decimal conversion
    /// </summary>
    public static class DoubleToDecimalExtension
    {
        public static decimal ToDecimal(this double value)
        {
            return new decimal(value);
        }
    }
}