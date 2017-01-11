namespace Solinor.MonthlyWageCalculation.Services
{
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.Calculations;
    using Solinor.MonthlyWageCalculation.Models;

    /// <summary>
    /// Wage Service Interface
    /// </summary>
    interface IWageService 
    {
        PersonnelWages CalculateWages(IWageCalculation wageCalculation, IHoursCalculation hoursCalculation);
        List<Person> GetPersons();
    } 
}