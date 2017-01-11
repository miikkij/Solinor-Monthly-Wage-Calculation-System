using System;
using System.Linq;
using System.IO;
using System.Text;
using Solinor.MonthlyWageCalculation.Calculations;
using Solinor.MonthlyWageCalculation.Csv;
using Solinor.MonthlyWageCalculation.Models;
using Solinor.MonthlyWageCalculation.Services;

class Program
{
    private static string[] titleLogo =
    {
        "\n\n\t _______________________________________ , .  ' ",
        "\t|   __|  _  |   |_____|   \\ |  _  |  _  |",
        "\t|___  |     |   |_    |     |     |   _//___Monthly ___Wage __  __ .  ' ",
        "\t|_____|_____|____||___|_\\___|_____|_\\_____Calculation__2017____>> >  .;; . \n\n\n\""
    };

    /// <summary>
    /// Company ascii logo as application title + logo
    /// </summary>
    public static void PrintTitle()
    {
        titleLogo.ToList().ForEach(s => Console.WriteLine(s));
    }

    /// <summary>
    /// Loads Csv formatted file
    /// </summary>
    /// <param name="filename"></param>
    /// <returns>string including all the data</returns>
    public static string LoadCsvFile(string filename)
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

    private static WageService wageService;

    static void Main(string[] args)
    {
        PrintTitle();

        // For the purpose of this test there is a possibility to load some another csv 
        // file also to test error handling and possible other hour entry lists
        var filename = args.Count() > 0 ? args[0] : @"HourList201403.csv";

        if (!File.Exists(filename))
        {
            Console.WriteLine("File '" + filename + "' not available");
            return;
        }

        // Initialize service to process and serve the data and calculation routines to do the precission calculation
        wageService = new WageService();

        try
        {
            wageService.UpdateDataFromCSV(LoadCsvFile(filename));
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

        var defaultWageCalculation = new DefaultWageCalculation("$", regularSalary, eveningSalary, overtime25multiplier,
             overtime50multiplier, overtime100multiplier);
        var defaultHourCalculation = new DefaultHoursCalculation();

        var personnelWages = wageService.CalculateWages(defaultWageCalculation, defaultHourCalculation);

        PrintPersonnelWageSlips(personnelWages);
    }

    private static void PrintPersonnelWageSlips(PersonnelWages personnelWages)
    {
        foreach (var person in wageService.GetPersons())
        {
            var wageSlips = personnelWages.GetMonthlyWageSlips(person);
            foreach (var wageSlip in wageSlips)
            {
                Console.WriteLine("--------- WageSlip [" + person.Name + " - " + wageSlip.Date.ToString("yyyy-MM") + "]-----------------------------\n");
                Console.WriteLine("\tRegular hours:\t " + wageSlip.GetTotalHours(HoursType.Regular).ToString("n2"));
                Console.WriteLine("\tEvening hours:\t " + wageSlip.GetTotalHours(HoursType.EveningWork).ToString("n2"));
                Console.WriteLine("\tOvertime hours:\t " + wageSlip.GetTotalHours(HoursType.Overtime).ToString("n2"));
                Console.WriteLine("\tTotal hours:\t " + wageSlip.GetTotalHours(HoursType.All).ToString("n2"));
                Console.WriteLine("\tTotal pay:\t $" + wageSlip.Totalpay().ToString("n2"));
                Console.WriteLine();                
                Console.WriteLine("--------- Hour list \n");

                foreach (var workDay in wageSlip.Days)
                {
                    Console.WriteLine("\t[" + workDay.Date.ToString("yyyy-MM-dd") + "]----------------------------------------------------");
                    foreach (var wageEntry in workDay.WageEntries())
                    {
                        Console.WriteLine("\t  > Desc: " + wageEntry.Description + "\t [Amount: " + wageEntry.Hours.ToString("n2") + "]" +  
                            "\tPayment: " + wageEntry.Currency + wageEntry.GetTotal().ToString("n2"));
                    }
                }

                Console.WriteLine("\n--------- End of hour list\n\n");
            }
        }
    }
}
