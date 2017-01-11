namespace Solinor.MonthlyWageCalculation.Models
{
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Personnel wages - Collage class
    /// </summary>
    public class PersonnelWages
    {
        private Dictionary<Person, List<WageSlip>> MonthlyWages = new Dictionary<Person, List<WageSlip>>();

        public void AddWageSlip(Person person, WageSlip monthlyWageSlip)
        {
            // Add new wageslip for person ID and if persons does 
            if (!this.MonthlyWages.ContainsKey(person))
            {
                this.MonthlyWages[person] = new List<WageSlip>();
            }
            this.MonthlyWages[person].Add(monthlyWageSlip);
        }

        public List<WageSlip> GetMonthlyWageSlips(Person person)
        {
            var result = new List<WageSlip>();
            if (MonthlyWages.ContainsKey(person))
            {
                result = MonthlyWages[person];
            }
            return result;
        }

        public List<WageSlip> GetMonthlyWageSlips()
        {
            return MonthlyWages.SelectMany(x => x.Value).ToList();
        }       
    }
}