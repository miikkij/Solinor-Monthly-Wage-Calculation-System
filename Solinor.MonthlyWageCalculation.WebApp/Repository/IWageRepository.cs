using System.Collections.Generic;
using Solinor.MonthlyWageCalculation.Models;

namespace MvcApp.Repository
{
    public interface IWageRepository
    {
        PersonnelWages GetPersonnelWages();
        IEnumerable<Person> GetPersonnel();
        Person GetPerson(string id);    
        IEnumerable<WageSlip> GetWagesByPersonId(string id);
    }
}