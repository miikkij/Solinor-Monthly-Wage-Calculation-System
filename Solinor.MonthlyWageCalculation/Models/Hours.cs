namespace Solinor.MonthlyWageCalculation.Models
{
    using System;

    /// <summary>
    /// Hours with start and end time. If end time is earlier than start then one day is added to end time.
    /// This way it is possible to almost have full 48 hour work shift.
    /// </summary>
    public class Hours
    {
        public Hours(DateTime startTime, DateTime endTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            if (endTime < startTime) EndTime = endTime.AddDays(1);
        }

        public DateTime StartTime { get; private set; }
        
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Total hours between start- and end-time.
        /// </summary>
        /// <returns>Exact total hours in decimal</returns>
        public decimal TotalHours 
        { 
            get 
            {
                return new Decimal(new TimeSpan(this.EndTime.Ticks-this.StartTime.Ticks).TotalHours);
            }
        }
    }
}
