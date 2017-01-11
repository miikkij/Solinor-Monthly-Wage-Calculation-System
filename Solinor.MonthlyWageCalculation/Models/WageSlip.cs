namespace Solinor.MonthlyWageCalculation.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Wage Slip for each month - Collage class
    /// </summary>
    public class WageSlip
    {
        public UInt64 personId; // lets keep this
        public List<WorkDay> WorkDays = new List<WorkDay>();

        public DateTime Date { get; private set; }

        public List<WorkDay> Days
        {
            get
            {
                return this.WorkDays;
            }
        }

        public WageSlip(UInt64 personId, DateTime date, List<WorkDay> workDays)
        {
            this.personId = personId;
            this.WorkDays = workDays;
            this.Date = date;
        }

        public decimal Totalpay() 
        {
            return this.WorkDays.Sum(x => x.TotalWage());
        }

        public decimal GetTotalHours(HoursType type)
        {
            return this.WorkDays.Sum(x => x.GetHours(type));
        }

        public string TotalPay
        {
            get 
            {
                return this.Totalpay().ToString("n2");
            }
        }

        public decimal TotalHoursAll
        {
            get 
            {
                return this.GetTotalHours(HoursType.All);
            }
        }        

        public decimal TotalHoursRegular
        {
            get 
            {
                return this.GetTotalHours(HoursType.Regular);
            }
        }     

        public decimal TotalHoursEvening
        {
            get 
            {
                return this.GetTotalHours(HoursType.EveningWork);
            }
        }        

        public decimal TotalHoursOvertime
        {
            get 
            {
                return this.GetTotalHours(HoursType.Overtime);
            }
        }                  
    }
}