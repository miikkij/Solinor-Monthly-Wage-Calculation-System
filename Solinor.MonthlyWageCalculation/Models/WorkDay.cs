namespace Solinor.MonthlyWageCalculation.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.Calculations;

    /// <summary>
    /// Workday encapsulates wage and hour calculation and gives wage entries, total hour and total wage when requested
    /// </summary>
    public class WorkDay
    {
        IWageCalculation WageCalculation { get; }
        
        IHoursCalculation HoursCalculation { get; }

        private List<Hours> Hours = new List<Hours>();

        public WorkDay(List<Hours> hours, IWageCalculation wageCalculation, IHoursCalculation hoursCalculation)
        {
            this.Hours = hours;
            this.WageCalculation = wageCalculation;
            this.HoursCalculation = hoursCalculation;
        }

        public decimal TotalHours()
        {
            return this.Hours.Sum(x => x.TotalHours);
        }

        public decimal GetHours(HoursType type)
        {
            return HoursCalculation.GetHours(type, this.Hours);
        }

        public List<WageEntry> WageEntries()
        {
            var wageEntry = WageCalculation.GetWageEntries(this.Hours, HoursCalculation);
            return wageEntry;
        }

        public decimal TotalWage()
        {
            return this.WageEntries().Sum(x => x.GetTotal());
        }

        public List<WageEntry> Entries
        {
            get
            {
                return this.WageEntries();
            }
        }

        public DateTime Date
        {
            get
            {
                return this.Hours.FirstOrDefault().StartTime;
            }
        }
    }
}