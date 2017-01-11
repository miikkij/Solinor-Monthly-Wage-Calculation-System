namespace Solinor.MonthlyWageCalculation.Csv
{
    using TinyCsvParser.Mapping;

    /// <summary>
    /// TinyCsvParser Csv Mapping class to CsvRowDataHourEntry model
    /// </summary>
    public class CsvRowDataHourEntryMapping : CsvMapping<CsvRowDataHourEntry>
    {
        public CsvRowDataHourEntryMapping()
            : base()
        {
            MapProperty(0, x => x.Name);
            MapProperty(1, x => x.Id);
            MapProperty(2, x => x.StartDateString);
            MapProperty(3, x => x.HoursStart);
            MapProperty(4, x => x.HoursEnd);            
        }
    }
}