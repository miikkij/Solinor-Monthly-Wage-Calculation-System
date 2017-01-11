
namespace Solinor.MonthlyWageCalculation.UnitTests
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    using Solinor.MonthlyWageCalculation.Calculations;
    using Solinor.MonthlyWageCalculation.Models;

    public class WageCalculationTests
    {
        public string CurrencyChar = @"$";
        public decimal RegularSalary = 3.75m;
        public decimal EveningSalary = 4.9m;
        public decimal Overtime25Multiplier = 1.25m;
        public decimal Overtime50Multiplier = 1.5m;
        public decimal Overtime100Multiplier = 2m;

        /// <summary>
        /// Helper to output information if necessary inside the tests when testing
        /// </summary>
        private readonly ITestOutputHelper debugOutput;
        public WageCalculationTests(ITestOutputHelper output)
        {
            debugOutput = output;
        }

        [Fact]
        public void ShouldCalculateNormalHoursFromSingleEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 16, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);

            Assert.True(regular.GetTotal() == (8m * RegularSalary));
        }

        [Fact]
        public void ShouldCalculateNormalAndEveningWorkCompensationHoursSingleFromSingleEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 20, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);
            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);

            Assert.True(regular.GetTotal() == (6m * RegularSalary));
            Assert.True(evening.GetTotal() == (2m * EveningSalary));
        }

        [Fact]
        public void ShouldCalculateNormalAndEarlyEveningWorkCompensationHoursSingleFromSingleEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 04, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);
            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);

            Assert.True(regular.GetTotal() == (6m * RegularSalary));
            Assert.True(evening.GetTotal() == (2m * EveningSalary));
        }

        [Fact]
        public void ShouldCalculateNormalAndEveningWorkCompensationHoursAndOvertimeCompensationHoursFromSingleEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 2, 04, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);
            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);
            var overtime25 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation25);
            var overtime50 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation50);
            var overtime100 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation100);

            Assert.True(regular.GetTotal() == (6m * RegularSalary));
            Assert.True(evening.GetTotal() == (2m * EveningSalary));
            Assert.True(overtime25.GetTotal() == (2m * (RegularSalary * Overtime25Multiplier)));
            Assert.True(overtime50.GetTotal() == (2m * (RegularSalary * Overtime50Multiplier)));
            Assert.True(overtime100.GetTotal() == (4m * (RegularSalary * Overtime100Multiplier)));
        }

        [Fact]
        public void ShouldCalculateNormalHoursFromMultipleEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 11, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 13, 0, 0), new DateTime(2017, 1, 1, 16, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);

            Assert.True(regular.GetTotal() == (6m * RegularSalary));
        }

        [Fact]
        public void ShouldCalculateNormalAndEveningWorkCompensationHoursFromMultipleEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 04, 0, 0), new DateTime(2017, 1, 1, 8, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 16, 0, 0), new DateTime(2017, 1, 1, 20, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);
            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);

            Assert.True(regular.GetTotal() == (4m * RegularSalary));
            Assert.True(evening.GetTotal() == (4m * EveningSalary));
        }

        [Fact]
        public void ShouldCalculateNormalAndEveningWorkCompensationHoursAndOvertimeCompensationHoursFromMultipleEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 04, 0, 0), new DateTime(2017, 1, 1, 8, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 16, 0, 0), new DateTime(2017, 1, 2, 01, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);
            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);
            var overtime25 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation25);
            var overtime50 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation50);
            var overtime100 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation100);

            Assert.True(regular.GetTotal() == (4m * this.RegularSalary));
            Assert.True(evening.GetTotal() == (4m * this.EveningSalary));
            Assert.True(overtime25.GetTotal() == (2m * this.RegularSalary * this.Overtime25Multiplier));
            Assert.True(overtime50.GetTotal() == (2m * this.RegularSalary * this.Overtime50Multiplier));
            Assert.True(overtime100.GetTotal() == (1m * this.RegularSalary * this.Overtime100Multiplier));
        }

        [Fact]
        public void ShouldCalculateEveningWorkCompensationHoursFromMultipleEntriesPassingToNextDay()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 04, 0, 0), new DateTime(2017, 1, 1, 06, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 18, 0, 0), new DateTime(2017, 1, 1, 20, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 21, 0, 0), new DateTime(2017, 1, 1, 22, 0, 0)));

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);

            Assert.True(evening.GetTotal() == (5m * EveningSalary));
        }

        [Fact]
        public void ShouldCalculateNormalAndEveningWorkCompensationHoursAndOvertimeCompensationHoursFromMultipleEntriesPassingToNextDay()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 3, 0, 0), new DateTime(2017, 1, 1, 4, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 5, 0, 0), new DateTime(2017, 1, 1, 7, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 9, 0, 0), new DateTime(2017, 1, 1, 10, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 17, 0, 0), new DateTime(2017, 1, 1, 19, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 23, 0, 0), new DateTime(2017, 1, 2, 05, 0, 0)));

            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            var defaultWagesCalculation = new DefaultWageCalculation(this.CurrencyChar, this.RegularSalary, this.EveningSalary, this.Overtime25Multiplier, this.Overtime50Multiplier, this.Overtime100Multiplier);

            var wageEntries = defaultWagesCalculation.GetWageEntries(hours, defaultHoursCalculation);

            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.Regular));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.EveningWorkCompensation));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation25));
            Assert.True(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation50));
            Assert.False(wageEntries.Any(entry => entry.Type == WageEntryType.OvertimeCompensation100));

            var regular = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.Regular);
            var evening = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.EveningWorkCompensation);
            var overtime25 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation25);
            var overtime50 = wageEntries.FirstOrDefault(entry => entry.Type == WageEntryType.OvertimeCompensation50);

            Assert.True(regular.GetTotal() == (3m * this.RegularSalary));
            Assert.True(evening.GetTotal() == (5m * this.EveningSalary));
            Assert.True(overtime25.GetTotal() == (2m * this.RegularSalary * this.Overtime25Multiplier));
            Assert.True(overtime50.GetTotal() == (2m * this.RegularSalary * this.Overtime50Multiplier));
        }
    }
}