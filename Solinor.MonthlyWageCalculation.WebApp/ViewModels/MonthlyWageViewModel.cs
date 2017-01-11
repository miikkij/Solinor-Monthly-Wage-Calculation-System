namespace Solinor.MonthlyWageCalculation.WebApp.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class MonthlyWageViewModel
    {
        public MonthlyWageViewModel()
        {
            this.PaymentEntries = new List<PaymentEntryViewModel>();
        }

        public PersonViewModel Person { get; set; }

        public Decimal TotalRegularHours { get; set; }

        public Decimal TotalEveningHours { get; set; }

        public Decimal TotalOvertimeHours { get; set; }

        public Decimal TotalHours { get; set; }

        public Decimal TotalPay { get; set; }

        public string TotalPayRounded
        {
            get
            {
                return this.TotalPay.ToString("n2");
            }
        }

        public DateTime Date
        {
            get
            {
                return this.PaymentEntries.FirstOrDefault().Date;
            }
        }

        public List<PaymentEntryViewModel> PaymentEntries { get; set; }
    }
}