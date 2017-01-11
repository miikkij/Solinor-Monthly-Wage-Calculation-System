namespace Solinor.MonthlyWageCalculation.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Person model 
    /// </summary>
    public class Person
    {
        public Person(UInt64 id, string name)
        {
            this.Id = id;
            this.Name = name;
        } 

        /// <summary>
        /// Identification
        /// </summary>
        /// <returns>Id</returns>
        public UInt64 Id { get; private set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Hour entries grouped by per day
        /// </summary>
        private Dictionary<DateTime, List<Hours>> HourEntries = new Dictionary<DateTime, List<Hours>>();

        /// <summary>
        /// Return HourEntries
        /// </summary>
        /// <returns>Hours list</returns>
        public List<Hours> GetHourEntries() 
        {
            return this.HourEntries.Select(dateTimeHourPair => dateTimeHourPair.Value).SelectMany(listHourEntries => listHourEntries).ToList();
        }

        /// <summary>
        /// Insert hours for single day. 
        /// </summary>
        public void InsertHours(DateTime day, DateTime startTime, DateTime endTime)
        {
            var hours = new Hours(startTime, endTime);
            if (!this.HourEntries.ContainsKey(day)) this.HourEntries[day] = new List<Hours>(); 
            this.HourEntries[day].Add(hours);
        }
    }
}