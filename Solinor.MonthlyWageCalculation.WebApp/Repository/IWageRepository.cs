namespace Solinor.MonthlyWageCalculation.WebApp.Repository
{
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.WebApp.ViewModels;

    public interface IWageRepository
    {
        IEnumerable<PersonViewModel> GetPersonnel();

        PersonViewModel GetPerson(string id);
    }
}