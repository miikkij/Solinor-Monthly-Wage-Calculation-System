namespace Solinor.MonthlyWageCalculation.WebApp.ViewModels
{
    using System;    

    public class PaymentEntryViewModel
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Decimal Hours { get; set; }
        public string HoursRounded 
        { 
            get 
            { 
                return this.Hours.ToString("n2"); 
            } 
        }
        public Decimal Payment { get; set; }
        public string PaymentRounded 
        { 
            get 
            { 
                return this.Payment.ToString("n2"); 
            } 
        }        
        public string Currency { get; set; }
    }
}
