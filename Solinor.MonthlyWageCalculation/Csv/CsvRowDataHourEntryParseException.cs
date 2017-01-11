namespace Solinor.MonthlyWageCalculation.Csv
{
    using System;

    /// <summary>
    /// Csv row data hour entry parse exception
    /// </summary>
    public class CsvRowDataHourEntryParseException : Exception
    {
        public CsvRowDataHourEntryParseException()
        {
        }

        public CsvRowDataHourEntryParseException(string message)
            : base(message)
        {
        }

        public CsvRowDataHourEntryParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}