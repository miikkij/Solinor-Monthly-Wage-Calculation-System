namespace Solinor.MonthlyWageCalculation.Csv
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Model for importing row data from CSV data
    /// </summary>
    public class CsvRowDataHourEntry
    {
        public UInt64 Id { get; private set; }

        public string Name { get; private set; }

        public string StartDateString { get; private set; }
        
        public DateTime StartDate
        {
            get
            {
                var date = DateTime.MinValue;
                var pattern = @"d.M.yyyy";
                DateTime.TryParseExact(this.StartDateString, pattern, null, DateTimeStyles.None, out date);
                return date;
            }
        }

        public string HoursStart { get; private set; }

        public string HoursEnd { get; private set; }
    }
}