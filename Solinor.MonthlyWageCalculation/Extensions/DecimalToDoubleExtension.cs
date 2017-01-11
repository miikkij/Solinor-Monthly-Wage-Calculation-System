namespace Solinor.MonthlyWageCalculation.Extensions
{
    /// <summary>
    /// Helper extension for decimal to do decimal to double conversion
    /// </summary>
    public static class DecimalToDoubleExtension
    {
        public static double ToDouble(this decimal value)
        {
            return (double)value;
        }
    }
}