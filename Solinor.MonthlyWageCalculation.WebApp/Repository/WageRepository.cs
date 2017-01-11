using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Text;

//using Microsoft.Extensions.PlatformAbstractions;

using Solinor.MonthlyWageCalculation.Csv;
using Solinor.MonthlyWageCalculation.Calculations;
using Solinor.MonthlyWageCalculation.Models;
using Solinor.MonthlyWageCalculation.Services;

namespace MvcApp.Repository
{
    public class WageRepository : IWageRepository
    {
        private static WageService wageService;
        private static DefaultWageCalculation defaultWageCalculation;
        private static DefaultHoursCalculation defaultHourCalculation;
        private static PersonnelWages personnelWages;

        public Person GetPerson(string id)
        {
            var person = wageService.GetPersons().FirstOrDefault(x => x.Id.ToString() == id);
            if (person == null) person = new Person(0, "");
            return person;
        }

        public IEnumerable<Person> GetPersonnel()
        {
            return wageService.GetPersons();
        }

        public PersonnelWages GetPersonnelWages()
        {
            return personnelWages;
        }

        public IEnumerable<WageSlip> GetWagesByPersonId(string id)
        {
            var wages = personnelWages.MonthlyWages.FirstOrDefault(x => x.Key.Id.ToString() == id).Value;
            if (wages == null) wages = new List<WageSlip>();
            return wages;
        }

        private string loadCsvFile(string filename)
        {
            var hourEntriesInCsvData = File.ReadAllLines(filename);

            // TinyCsv couldn't read raw string, this is a workaround for now
            var stringBuilderCsv = new StringBuilder();
            foreach (string line in hourEntriesInCsvData)
            {
                stringBuilderCsv.AppendLine(line);
            }

            return stringBuilderCsv.ToString();
        }


        public WageRepository()
        {
/*            var pathToFile = env.ApplicationBasePath 
            + Path.DirectorySeparatorChar.ToString() 
            + "yourfolder"
            + Path.DirectorySeparatorChar.ToString() 
            + "yourfilename.txt";

            string fileContent;*/

            var filename = @"HourList201403.csv";

            if (!File.Exists(filename))
            {
                Console.WriteLine("File '" + filename + "' not available");
                return;
            }

            // Initialize service to process and serve the data and calculation routines to do the precission calculation
            wageService = new WageService();

            try
            {
                wageService.UpdateDataFromCSV(loadCsvFile(filename));
            } 
            catch (CsvRowDataHourEntryParseException exception)
            {
                Console.WriteLine("Error while trying to parse CSV data");
                Console.WriteLine("Exception.Message: " + exception.Message);
                Console.WriteLine("Exception.InnerException: " + exception.InnerException);
                Console.WriteLine("Exception.StackTrace: " + exception.StackTrace);
                return;
            }

            decimal regularSalary = 3.75m;
            decimal eveningSalary = regularSalary + 1.15m;
            decimal overtime25multiplier = 1.25m;
            decimal overtime50multiplier = 1.5m;
            decimal overtime100multiplier = 2.0m;

            defaultWageCalculation = new DefaultWageCalculation("$", regularSalary, eveningSalary, overtime25multiplier,
                overtime50multiplier, overtime100multiplier);
            defaultHourCalculation = new DefaultHoursCalculation();

            personnelWages = wageService.CalculateWages(defaultWageCalculation, defaultHourCalculation);
        }
    }
}