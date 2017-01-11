using System.Collections.Generic;
using Solinor.MonthlyWageCalculation.WebApp.ViewModels;

namespace Solinor.MonthlyWageCalculation.WebApp.Repository
{
    public interface IWageRepository
    {
        IEnumerable<PersonViewModel> GetPersonnel();
        PersonViewModel GetPerson(string id);    
    }
}