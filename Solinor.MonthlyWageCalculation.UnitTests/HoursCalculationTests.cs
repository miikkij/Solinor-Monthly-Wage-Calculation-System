namespace Solinor.MonthlyWageCalculation.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    using Solinor.MonthlyWageCalculation.Calculations;
    using Solinor.MonthlyWageCalculation.Models;

    public class HourCalculationTests
    {
        /// <summary>
        /// Helper to output information if necessary inside the tests when testing
        /// </summary>
        private readonly ITestOutputHelper debugOutput;
        public HourCalculationTests(ITestOutputHelper output)
        {
            debugOutput = output;
        }

        [Fact]
        public void ShouldCalculateAllHours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);

            Assert.True(calculatedHoursAll == 12.0m);
        }

        [Fact]
        public void ShouldCalculateAllTypesOfHours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 12.0m);
            Assert.True(calculatedHoursEve == 6.0m);
            Assert.True(calculatedHoursReg == 2.0m);
            Assert.True(calculatedHoursOt == 4.0m);
        }

        #region regular hour tests

        [Fact]
        public void ShouldCalculateRegularMaxHours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 16, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 8.0m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularMaxHoursStartingFrom0600Hours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 6, 0, 0), new DateTime(2017, 1, 1, 14, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 8.0m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularMaxEndingTo0800Hours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 10, 0, 0), new DateTime(2017, 1, 1, 18, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 8.0m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularMaxFromMultipleEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 6, 0, 0), new DateTime(2017, 1, 1, 08, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 09, 0, 0), new DateTime(2017, 1, 1, 10, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 11, 0, 0), new DateTime(2017, 1, 1, 16, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 8.0m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }



        [Fact]
        public void ShouldCalculateRegularMinEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 8, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 0.25m);
            Assert.True(calculatedHoursReg == 0.25m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularMidRange()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 4.0m);
            Assert.True(calculatedHoursReg == 4.0m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateRegularMidRangeHalfHour()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 12, 30, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 4.5m);
            Assert.True(calculatedHoursReg == 4.5m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateRegularMidRangeFromMultipleQuarterHourEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 8, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 9, 0, 0), new DateTime(2017, 1, 1, 9, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 10, 0, 0), new DateTime(2017, 1, 1, 10, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 0.75m);
            Assert.True(calculatedHoursReg == 0.75m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        #endregion

        #region Evening compensation hour tests

        [Fact]
        public void ShouldCalculateEveningMaxHours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 18, 0, 0), new DateTime(2017, 1, 2, 2, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 8.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateEveningMaxHoursStartingFrom0000Hours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2017, 1, 1, 6, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 6.0m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 6.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateEveningMaxEndingTo0600HoursNextDay()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 22, 0, 0), new DateTime(2017, 1, 2, 06, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 8.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateEveningMaxFromMultipleEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2017, 1, 1, 6, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 18, 0, 0), new DateTime(2017, 1, 1, 20, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 8.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }



        [Fact]
        public void ShouldCalculateEveningMinEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2017, 1, 1, 0, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 0.25m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 0.25m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateEveningMidRange()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 19, 0, 0), new DateTime(2017, 1, 1, 22, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 3.0m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 3.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateEveningMidRangeHalfHour()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 19, 0, 0), new DateTime(2017, 1, 1, 23, 30, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 4.5m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 4.5m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateEveningMidRangeFromMultipleQuarterHourEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 18, 0, 0), new DateTime(2017, 1, 1, 18, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 19, 0, 0), new DateTime(2017, 1, 1, 19, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 20, 0, 0), new DateTime(2017, 1, 1, 20, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 0.75m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 0.75m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        #endregion

        #region Regular and evening hour tests for single workday

        [Fact]
        public void ShouldCalculateRegularAndEveningHours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 16, 0, 0), new DateTime(2017, 1, 1, 20, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 4.0m);
            Assert.True(calculatedHoursReg == 2.0m);
            Assert.True(calculatedHoursEve == 2.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularAlmostMaxAndMinimumEveningHoursEndingTo1815Hours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 10, 15, 0), new DateTime(2017, 1, 1, 18, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 7.75m);
            Assert.True(calculatedHoursEve == 0.25m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateRegularAndEarlyEveningHours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 05, 0, 0), new DateTime(2017, 1, 1, 10, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            debugOutput.WriteLine("calculatedHoursAll:" + calculatedHoursAll);
            debugOutput.WriteLine("calculatedHoursReg:" + calculatedHoursReg);
            debugOutput.WriteLine("calculatedHoursEve:" + calculatedHoursEve);
            debugOutput.WriteLine("calculatedHoursOt:" + calculatedHoursOt);

            Assert.True(calculatedHoursAll == 5.0m);
            Assert.True(calculatedHoursReg == 4.0m);
            Assert.True(calculatedHoursEve == 1.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularMinimumAndMaximumEveningHoursStartingFrom1745Hours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 17, 45, 0), new DateTime(2017, 1, 2, 01, 45, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 0.25m);
            Assert.True(calculatedHoursEve == 7.75m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularAndEveningHoursFromMultipleEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 04, 0, 0), new DateTime(2017, 1, 1, 08, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 16, 0, 0), new DateTime(2017, 1, 1, 20, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 4.0m);
            Assert.True(calculatedHoursEve == 4.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }



        [Fact]
        public void ShouldCalculateRegularAndEveningMinEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2017, 1, 1, 0, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 6, 0, 0), new DateTime(2017, 1, 1, 6, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 0.5m);
            Assert.True(calculatedHoursReg == 0.25m);
            Assert.True(calculatedHoursEve == 0.25m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularAndEveningHoursFromMidRange()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 1, 0, 0), new DateTime(2017, 1, 1, 3, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 10, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 19, 0, 0), new DateTime(2017, 1, 1, 22, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 7.0m);
            Assert.True(calculatedHoursReg == 2.0m);
            Assert.True(calculatedHoursEve == 5.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateRegularAndEveningMidRangeHalfHour()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 10, 0, 0), new DateTime(2017, 1, 1, 12, 30, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 19, 30, 0), new DateTime(2017, 1, 1, 23, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 6.0m);
            Assert.True(calculatedHoursReg == 2.5m);
            Assert.True(calculatedHoursEve == 3.5m);
            Assert.True(calculatedHoursOt == 0.0m);
        }

        [Fact]
        public void ShouldCalculateRegularAndEveningMidRangeFromMultipleQuarterHourEntries()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 8, 0, 0), new DateTime(2017, 1, 1, 8, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 9, 0, 0), new DateTime(2017, 1, 1, 9, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 10, 0, 0), new DateTime(2017, 1, 1, 10, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 18, 0, 0), new DateTime(2017, 1, 1, 18, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 19, 0, 0), new DateTime(2017, 1, 1, 19, 15, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 20, 0, 0), new DateTime(2017, 1, 1, 20, 15, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 1.5m);
            Assert.True(calculatedHoursReg == 0.75m);
            Assert.True(calculatedHoursEve == 0.75m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        [Fact]
        public void ShouldCalculateRegularAndEveningMaxToNextDayMultipleEntry()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 17, 0, 0), new DateTime(2017, 1, 1, 18, 0, 0)));
            hours.Add(new Hours(new DateTime(2017, 1, 1, 23, 0, 0), new DateTime(2017, 1, 2, 06, 0, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 8.0m);
            Assert.True(calculatedHoursReg == 1.0m);
            Assert.True(calculatedHoursEve == 7.0m);
            Assert.True(calculatedHoursOt == 0.0m);
        }


        #endregion

        #region Overtime tests


        [Fact]
        public void ShouldCalculateRegularAndOvertime()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 10, 0, 0), new DateTime(2017, 1, 1, 19, 00, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 9.0m);
            Assert.True(calculatedHoursReg == 8.0m);
            Assert.True(calculatedHoursEve == 0.0m);
            Assert.True(calculatedHoursOt == 1.0m);
        }


        [Fact]
        public void ShouldCalculateRegularAndEveningAndOvertime25()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 11, 0, 0), new DateTime(2017, 1, 1, 20, 00, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 9.0m);
            Assert.True(calculatedHoursReg == 7.0m);
            Assert.True(calculatedHoursEve == 1.0m);
            Assert.True(calculatedHoursOt == 1.0m);
        }

        [Fact]
        public void ShouldCalculateRegularAndEveningAndOvertimeFromStarting0500Hours()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 05, 0, 0), new DateTime(2017, 1, 1, 14, 00, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);



            Assert.True(calculatedHoursAll == 9.0m);
            Assert.True(calculatedHoursReg == 7.0m);
            Assert.True(calculatedHoursEve == 1.0m);
            Assert.True(calculatedHoursOt == 1.0m);
        }

        [Fact]
        public void ShouldCalculateEveningAndOvertime()
        {
            var defaultHoursCalculation = new DefaultHoursCalculation();
            var hours = new List<Hours>();
            hours.Add(new Hours(new DateTime(2017, 1, 1, 18, 0, 0), new DateTime(2017, 1, 2, 04, 00, 0)));
            var calculatedHoursAll = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.All, hours);
            var calculatedHoursReg = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Regular, hours);
            var calculatedHoursEve = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.EveningWork, hours);
            var calculatedHoursOt = ((IHoursCalculation)defaultHoursCalculation).GetHours(HoursType.Overtime, hours);

            Assert.True(calculatedHoursAll == 10.0m);
            Assert.True(calculatedHoursReg == 0.0m);
            Assert.True(calculatedHoursEve == 8.0m);
            Assert.True(calculatedHoursOt == 2.0m);
        }

        #endregion

        public void ShouldCalculateNormalAndEveningWorkHoursAndOvertimeHoursFromMultipleEntriesPassingToNextDay()
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

            Assert.True(calculatedHoursAll == 12.0m);
            Assert.True(calculatedHoursReg == 3.0m);
            Assert.True(calculatedHoursEve == 5.0m);
            Assert.True(calculatedHoursOt == 5.0m);
        }
    }
}