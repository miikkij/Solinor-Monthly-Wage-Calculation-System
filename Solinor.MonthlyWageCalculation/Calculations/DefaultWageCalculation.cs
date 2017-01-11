namespace Solinor.MonthlyWageCalculation.Calculations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.Extensions;
    using Solinor.MonthlyWageCalculation.Models;

    public class DefaultWageCalculation : IWageCalculation
    {
        decimal regularSalary;
        decimal eveningSalary;
        decimal overtime25multiplier;
        decimal overtime50multiplier;
        decimal overtime100multiplier;
        string currencyChar;

        /// <summary>
        /// Default wage calculation
        /// </summary>
        /// <param name="currencyChar">Currency character for e.g $</param>
        /// <param name="regularHourSalary">Regular salary per hour</param>
        /// <param name="eveningHourSalary">Evening salary per hour</param>
        /// <param name="overtime25multiplier">Overtime multiplier for first two hours</param>
        /// <param name="overtime50multiplier">Overtime multiplier for next two hours</param>
        /// <param name="overtime100multiplier">Overtime multiplier for rest of the hours</param>
        public DefaultWageCalculation(string currencyChar, decimal regularHourSalary, decimal eveningHourSalary, 
            decimal overtime25multiplier, decimal overtime50multiplier, decimal overtime100multiplier)
        {
            this.currencyChar = currencyChar;
            this.regularSalary = regularHourSalary;
            this.eveningSalary = eveningHourSalary;
            this.overtime25multiplier = overtime25multiplier;
            this.overtime50multiplier = overtime50multiplier;
            this.overtime100multiplier = overtime100multiplier;
        }

        /// <summary>
        /// Get wage entries
        /// </summary>
        /// <param name="hours">Hours list for calculations</param>
        /// <param name="hoursCalculation">Hour calculation</param>
        /// <returns>List of wage entries</returns>
        public List<WageEntry> GetWageEntries(List<Hours> hours, IHoursCalculation hoursCalculation)
        {
            var results = new List<WageEntry>();

            decimal regularHours = hoursCalculation.GetHours(HoursType.Regular, hours);
            decimal eveningCompensationHours = hoursCalculation.GetHours(HoursType.EveningWork, hours);
            decimal overtimeCompensationHours = hoursCalculation.GetHours(HoursType.Overtime, hours);

            if (regularHours > 0.0m)
            {
                var WageEntry = new WageEntry("Regular working hours", currencyChar, hours.FirstOrDefault().StartTime,
                    WageEntryType.Regular, regularHours, regularSalary, 1.0m);
                results.Add(WageEntry);
            }

            if (eveningCompensationHours > 0.0m)
            {
                var WageEntry = new WageEntry("Evening working hours", currencyChar, hours.FirstOrDefault().StartTime,
                    WageEntryType.EveningWorkCompensation, eveningCompensationHours, eveningSalary, 1.0m);
                results.Add(WageEntry);
            }

            if (overtimeCompensationHours.ToDouble() > 0.0)
            {
                var overtime25hours = overtimeCompensationHours.ToDouble() > 2.0 ? 2.0m : overtimeCompensationHours;
                var overtime50hours = overtimeCompensationHours.ToDouble() > 4.0 ? 2.0m : overtimeCompensationHours - 2.0m;
                var overtime100hours = overtimeCompensationHours - 4.0m;

                var overtime25WageEntry = new WageEntry("Overtime 25% hours", currencyChar, hours.FirstOrDefault().StartTime,
                    WageEntryType.OvertimeCompensation25, overtime25hours, regularSalary, overtime25multiplier);
                results.Add(overtime25WageEntry);

                if (overtime50hours > 0.0m)
                {
                    var overtime50WageEntry = new WageEntry("Overtime 50% hours", currencyChar, hours.FirstOrDefault().StartTime,
                        WageEntryType.OvertimeCompensation50, overtime50hours, regularSalary, overtime50multiplier);
                    results.Add(overtime50WageEntry);
                }

                if (overtime100hours > 0.0m)
                {
                    var overtime100WageEntry = new WageEntry("Overtime 100% hours", currencyChar, hours.FirstOrDefault().StartTime,
                        WageEntryType.OvertimeCompensation100, overtime100hours, regularSalary, overtime100multiplier);
                    results.Add(overtime100WageEntry);
                }
            }

            return results;
        }
    }
}