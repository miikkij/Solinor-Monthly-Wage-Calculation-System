namespace Solinor.MonthlyWageCalculation.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    using Solinor.MonthlyWageCalculation.Calculations;
    using Solinor.MonthlyWageCalculation.Csv;
    using Solinor.MonthlyWageCalculation.Models;

    /// <summary>
    /// Wage service parses 
    /// </summary>
    public class WageService : IWageService
    {
        private Dictionary<UInt64, Person> persons = new Dictionary<UInt64, Person>();

        /// <summary>
        /// Parse hours entries
        /// </summary>
        /// <param name="hourEntryRows"></param>
        /// <returns>list of catched exceptions while parsing date times and csv data</returns>
        private List<CsvRowDataHourEntryParseException> ParseHourEntries(List<CsvMappingResult<CsvRowDataHourEntry>> hourEntryRows)
        {
            List<CsvRowDataHourEntryParseException> catchedExceptions = new List<CsvRowDataHourEntryParseException>();

            foreach (var hourEntryRow in hourEntryRows)
            {
                if (!hourEntryRow.IsValid)
                {
                    catchedExceptions.Add(new CsvRowDataHourEntryParseException(@"Column[" + hourEntryRow.Error.ColumnIndex + "]: " + hourEntryRow.Error));
                    continue;
                }

                var hourEntryData = hourEntryRow.Result;

                var person = new Person(hourEntryData.Id, hourEntryData.Name);
                // Check if person already exists                    
                if (persons.ContainsKey(person.Id))
                {
                    person = persons[person.Id];
                }

                var pattern = @"d.M.yyyy H:m";
                var startDateTime = DateTime.Now;
                var startDateTimeInputString = hourEntryData.StartDateString + " " + hourEntryData.HoursStart;
                bool parsingSuccess = DateTime.TryParseExact(startDateTimeInputString, pattern, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime);

                if (!parsingSuccess)
                {
                    catchedExceptions.Add(new CsvRowDataHourEntryParseException(@"Parsing start date time failed: " + startDateTimeInputString));
                    continue;
                }

                var endDateTime = DateTime.Now;
                var endDateTimeInputString = hourEntryData.StartDateString + " " + hourEntryData.HoursEnd;
                parsingSuccess = DateTime.TryParseExact(endDateTimeInputString, pattern, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime);
                if (!parsingSuccess)
                {
                    catchedExceptions.Add(new CsvRowDataHourEntryParseException(@"Parsing end date time failed: " + endDateTimeInputString));
                    continue;
                }

                person.InsertHours(hourEntryData.StartDate, startDateTime, endDateTime);

                persons[person.Id] = person;
            }

            return catchedExceptions;
        }

        public void UpdateDataFromCSV(string csv)
        {
            persons.Clear();

            CsvParserOptions csvParserOptions = new CsvParserOptions(true, new[] { ',' });
            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            CsvRowDataHourEntryMapping csvMapper = new CsvRowDataHourEntryMapping();
            CsvParser<CsvRowDataHourEntry> csvParser = new CsvParser<CsvRowDataHourEntry>(csvParserOptions, csvMapper);

            var resultRows = csvParser
                .ReadFromString(csvReaderOptions, csv)
                .ToList();

            // Catch all the exceptions to own list if any occurs when doing data parsing to help debuging broken data
            List<CsvRowDataHourEntryParseException> catchedExceptions = ParseHourEntries(resultRows);

            // Throw all catched exceptions for further handling
            foreach(var exception in catchedExceptions)
            {
                throw exception;
            }
        }    

        /// <summary>
        /// Calculate wages
        /// </summary>
        /// <param name="wageCalculation">Wage calculations</param>
        /// <param name="hoursCalculation">Hour calculations</param>
        /// <returns>Personnel wages</returns>
        public PersonnelWages CalculateWages(IWageCalculation wageCalculation, IHoursCalculation hoursCalculation)
        {
            var personnelWages = new PersonnelWages();

            foreach(var idPersonPair in persons)
            {
                var personId = idPersonPair.Key;
                var person = idPersonPair.Value;

                // For each person go through hours and create work days from those hours. 
                var monthlyWorkdays = new Dictionary<DateTime, List<WorkDay>>();
                
                var workhoursEntriesPerDay = new Dictionary<DateTime, List<Hours>>();
                foreach(var hour in person.GetHourEntries())
                {
                    var dayDateTime = new DateTime(hour.StartTime.Year, hour.StartTime.Month, hour.StartTime.Day);
                    if (!workhoursEntriesPerDay.ContainsKey(dayDateTime))
                    {
                        workhoursEntriesPerDay[dayDateTime] = new List<Hours>();
                    }
                    workhoursEntriesPerDay[dayDateTime].Add(hour);
                }

                // Create Workdays
                foreach(var workhourEntries in workhoursEntriesPerDay)
                {
                    var workday = new WorkDay(workhourEntries.Value, wageCalculation, hoursCalculation);

                    var monthDateTime = new DateTime(workhourEntries.Key.Year, workhourEntries.Key.Month, 1);
                    if (!monthlyWorkdays.ContainsKey(monthDateTime))
                    {
                        monthlyWorkdays[monthDateTime] = new List<WorkDay>();
                    }
                    monthlyWorkdays[monthDateTime].Add(workday);
                }
                
                // Create WageSlips 
                foreach(var monthlyWorkday in monthlyWorkdays)
                {
                    var date = monthlyWorkday.Key;
                    var workdays = monthlyWorkday.Value;
                    var wageSlip = new WageSlip(personId, date, workdays);
                    personnelWages.AddWageSlip(person, wageSlip);
                }
            }

            return personnelWages;
        }

        /// <summary>
        /// Get persons list
        /// </summary>
        /// <returns>List of persons</returns>
        public List<Person> GetPersons()
        {
            return persons.Select(idPersonPair => idPersonPair.Value).ToList();
        }    
    }
}