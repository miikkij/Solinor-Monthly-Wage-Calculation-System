namespace Solinor.MonthlyWageCalculation.Calculations
{
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.Models;

    /// <summary>
    /// Wage calculation interface
    /// </summary>
    public interface IWageCalculation
    {
        List<WageEntry> GetWageEntries(List<Hours> hours, IHoursCalculation hoursCalculation);
    }
}