namespace Solinor.MonthlyWageCalculation.Calculations
{
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.Models;

    /// <summary>
    /// Hours calculation interface
    /// </summary>
    public interface IHoursCalculation
    {
        decimal GetHours(HoursType type, List<Hours> hours);
    }
}