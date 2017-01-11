namespace Solinor.MonthlyWageCalculation.WebApp.ViewModels
{
    using System.Collections.Generic;

    public class PersonViewModel
    {
        public PersonViewModel()
        {
            this.MonthlyWages = new List<MonthlyWageViewModel>();
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public List<MonthlyWageViewModel> MonthlyWages { get; set; }
    }
}