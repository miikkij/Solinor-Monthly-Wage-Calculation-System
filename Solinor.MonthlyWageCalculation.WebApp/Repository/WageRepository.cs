namespace Solinor.MonthlyWageCalculation.WebApp.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Text;

    using Solinor.MonthlyWageCalculation.Csv;
    using Solinor.MonthlyWageCalculation.Calculations;
    using Solinor.MonthlyWageCalculation.Models;
    using Solinor.MonthlyWageCalculation.Services;
    using Solinor.MonthlyWageCalculation.WebApp.ViewModels;

    public class WageRepository : IWageRepository
    {
        private static WageService wageService;
        private static DefaultWageCalculation defaultWageCalculation;
        private static DefaultHoursCalculation defaultHourCalculation;
        private static PersonnelWages personnelWages;

        public PersonViewModel GetPerson(string id)
        {
            var personViewModel = new PersonViewModel();

            var person = wageService.GetPersons().FirstOrDefault(x => x.Id.ToString() == id);
            if (person != null) 
            {
                var wages = personnelWages.MonthlyWages.FirstOrDefault(x => x.Key.Id.ToString() == id).Value;
                personViewModel.Id = person.Id;
                personViewModel.Name = person.Name;
                foreach(var wage in wages)
                {
                    var monthlyWageViewModel = new MonthlyWageViewModel();
                    personViewModel.MonthlyWages.Add(monthlyWageViewModel);
                    monthlyWageViewModel.TotalEveningHours = wage.GetTotalHours(HoursType.EveningWork);
                    monthlyWageViewModel.TotalHours = wage.GetTotalHours(HoursType.All);
                    monthlyWageViewModel.TotalRegularHours = wage.GetTotalHours(HoursType.Regular);
                    monthlyWageViewModel.TotalOvertimeHours = wage.GetTotalHours(HoursType.Overtime);
                    monthlyWageViewModel.TotalPay = wage.Totalpay();
                    monthlyWageViewModel.Person = personViewModel;
                    foreach(var workday in wage.WorkDays)
                    {
                        foreach(var wageEntry in workday.WageEntries())
                        {
                            var paymentEntry = new PaymentEntryViewModel();
                            paymentEntry.Currency = wageEntry.Currency;
                            paymentEntry.Date = wageEntry.HourEntryStartDate;
                            paymentEntry.Description = wageEntry.Description;
                            paymentEntry.Hours = wageEntry.Hours;
                            paymentEntry.Payment = wageEntry.GetTotal();
                            monthlyWageViewModel.PaymentEntries.Add(paymentEntry);
                        }
                    }
                }
            }

            return personViewModel;
        }

        public IEnumerable<PersonViewModel> GetPersonnel()
        {
            var personViewModels = new List<PersonViewModel>();

            foreach(var personsWageSlipsKeyValuePair in personnelWages.MonthlyWages)
            {
                var person = personsWageSlipsKeyValuePair.Key;
                var personViewModel = this.GetPerson(person.Id.ToString());
                personViewModels.Add(personViewModel);
            }

            return personViewModels;
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
            // User CSV file as a source repository;
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