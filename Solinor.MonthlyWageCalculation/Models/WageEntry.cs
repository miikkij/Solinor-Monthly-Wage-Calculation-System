namespace Solinor.MonthlyWageCalculation.Models
{
    using System;

    /// <summary>
    /// Wage entry for wage slip and reporting purposes
    /// </summary>
    public class WageEntry
    {
        public WageEntry(string description, string currency, DateTime hourEntryDate, WageEntryType wageEntryType, decimal hours, decimal originalPayPerHour, decimal compensationMultiplier)
        {
            this.Description = description;
            this.Currency = currency;
            this.HourEntryStartDate = hourEntryDate;
            this.Hours = hours;
            this.Type = wageEntryType;
            this.OriginalPayPerHour = originalPayPerHour;
            this.CompensationMultiplier = compensationMultiplier;
        }

        public DateTime HourEntryStartDate { get; private set; }   // from which day the entry becomes from

        public WageEntryType Type { get; private set; } // Row code, what it is, is it basic salary, overtime, full overtime, special and so on. 

        public string Currency { get; private set; } // $, eur etc.

        public string Description { get; private set; }

        public Decimal Hours { get; private set; }

        public Decimal OriginalPayPerHour { get; private set; }

        public Decimal CompensationMultiplier { get; private set; }

        // getters for getting the different values like pay without compensation, compensated pay 
        public Decimal GetBasePart()
        {
            // For currency calculations round dollar amounts to the nearest cent.
            return Decimal.Multiply(this.OriginalPayPerHour, this.Hours);  
        }

        public Decimal GetCompensatedPart()
        {
            // For currency calculations round dollar amounts to the nearest cent.
            return this.GetTotal() - this.GetBasePart();
        }

        public Decimal GetTotal()
        {
            return this.OriginalPayPerHour * this.CompensationMultiplier * this.Hours;
        }

        public string Total
        {
            get 
            {
                return this.GetTotal().ToString("n2");
            }
        }
    }
}