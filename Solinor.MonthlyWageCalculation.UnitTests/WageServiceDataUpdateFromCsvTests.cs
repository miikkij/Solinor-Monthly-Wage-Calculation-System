namespace Solinor.MonthlyWageCalculation.UnitTests
{
    using System;
    using System.Linq;
    using System.Text;
    using Xunit;

    using Solinor.MonthlyWageCalculation.Services;
    using Solinor.MonthlyWageCalculation.Csv;

    public class WageServiceDataUpdateFromCsvTests
    {
        [Fact]
        public void ShouldCreatePersonnelDataSuccessfully()
        {
            var csvData = new StringBuilder()
                .AppendLine("Person Name,Person ID,Date,Start,End")
                .AppendLine("Scott Scala,2,2.3.2014,6:00,14:00")
                .AppendLine("Janet Java,1,3.3.2014,9:30,17:00")
                .AppendLine("Larry Lolcode,3,3.3.2014,18:00,19:00");

            // Check name
            var wageService = new WageService();
            wageService.UpdateDataFromCSV(csvData.ToString());
            var person = wageService.GetPersons().First();

            // Check Id
            Assert.Equal<ulong>(2, person.Id);
            Assert.Equal<string>("Scott Scala", person.Name);

            // Check count of entries
            Assert.Equal<int>(3, wageService.GetPersons().Count);

            // Check start date
            Assert.Equal<DateTime>(new DateTime(2014,3,2,6,0,0), person.GetHourEntries().First().StartTime);
        }

        [Fact]
        public void ShouldImportMultipleHourEntriesForSingleDay()
        {
            var csvData = new StringBuilder()
                .AppendLine("Person Name,Person ID,Date,Start,End")
                .AppendLine("Scott Scala,2,2.3.2014,6:00,14:00")
                .AppendLine("Scott Scala,2,2.3.2014,8:30,16:30");

            // Check name
            var wageService = new WageService();
            wageService.UpdateDataFromCSV(csvData.ToString());
            var person = wageService.GetPersons().First();

            // Check count of entries, more than 1>
            Assert.True(person.GetHourEntries().Count>1);
        }

        [Fact]
        public void ShouldRaiseExceptionAndShowErrorenousDataAfterLoading()
        {
            // Data set with broken rows (missing data or wrong type, text instead of number)
            var csvData = new StringBuilder()
                .AppendLine("Person Name,Person ID,Date,Start,End")
                .AppendLine("Scott Scala332,2.3.2014,6:00,14:00")
                .AppendLine("Scott Scala,2,2.3.2014,8:30,16:30");

            // Check name
            var wageService = new WageService();
            Assert.Throws<CsvRowDataHourEntryParseException>(() => wageService.UpdateDataFromCSV(csvData.ToString()));
        }

        [Fact]
        public void ShouldRaiseExceptionIfDateTimeFormatIsNotAccepted()
        {
            // Data set with broken date time format
            // Data set with broken rows (missing data or wrong type, text instead of number)
            var csvData = new StringBuilder()
                .AppendLine("Person Name,Person ID,Date,Start,End")
                .AppendLine("Scott Scala,2,2/3/2014,6:00,14:00")
                .AppendLine("Scott Scala,2,2.3.2014,8:30,16:30");

            // Check name
            var wageService = new WageService();
            Assert.Throws<CsvRowDataHourEntryParseException>(() => wageService.UpdateDataFromCSV(csvData.ToString()));
        }

        [Fact]
        public void ShouldExtractHourEntryForEachPerson()
        {
            // Data set with broken date time format
            var csvData = new StringBuilder()
                .AppendLine("Person Name,Person ID,Date,Start,End")
                .AppendLine("Scott Scala,2,2.3.2014,6:00,14:00")
                .AppendLine("Janet Java,1,3.3.2014,9:30,17:00")
                .AppendLine("Larry Lolcode,3,3.3.2014,18:00,19:00");

            // Check name
            var wageService = new WageService();
            wageService.UpdateDataFromCSV(csvData.ToString());
            foreach(var person in wageService.GetPersons())
            {
                Assert.True(person.GetHourEntries().Count>=1);
            }
        }
    }
}