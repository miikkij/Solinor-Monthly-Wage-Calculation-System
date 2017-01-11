namespace Solinor.MonthlyWageCalculation.Calculations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Solinor.MonthlyWageCalculation.Extensions;
    using Solinor.MonthlyWageCalculation.Models;

    /// <summary>
    /// Note: This hour calculation only works if there isn't any overlapping entries for same day
    /// </summary>
    public class DefaultHoursCalculation : IHoursCalculation
    {
        public decimal GetHours(HoursType type, List<Hours> hours)
        {
            var result = 0.0m;

            var sortedHoursList = hours.OrderBy(hour => hour.StartTime);

            // Internal variables
            var totalHours = hours.Sum(x => x.TotalHours);
            var overtimeHours = 0.0m;
            var regularHours = 0.0m;
            var eveningHours = 0.0m;

            bool overTime = false;
            var overTimeLimit = 8.0m;

            // Go through the sorted hour list and gather different hour types from single or multiple hour entries
            // Normal working hours are from 06 to 18 and if work shift passes midnight then hours are taken into account 
            // for same day. If there are early hour shifts between 00-06, those are calculated as evening compensation hours.
            // If evening and regular working hours together passes the maximum amount of 8 hours then rest of the hours are 
            // counted as overtime hours. 
            foreach (var h in sortedHoursList)
            {
                // Create 06-18 range for comparison purposes for correct date
                var eveningStartTime = new DateTime(h.StartTime.Year, h.StartTime.Month, h.StartTime.Day, 0, 00, 00);
                var regularStartTime = new DateTime(h.StartTime.Year, h.StartTime.Month, h.StartTime.Day, 6, 00, 00);
                var regularEndTime = new DateTime(h.StartTime.Year, h.StartTime.Month, h.StartTime.Day, 18, 00, 00);
                var eveningEndTime = new DateTime(h.StartTime.Year, h.StartTime.Month, h.StartTime.Day, 06, 00, 00);
                eveningEndTime = eveningEndTime.AddDays(1.0);

                // Early evening hours check hours between 00-06
                if ((h.StartTime >= eveningStartTime) && (h.EndTime <= regularStartTime))
                {
                    var timeDiffHours = new TimeSpan(h.EndTime.Ticks - h.StartTime.Ticks).TotalHours.ToDecimal();
                    eveningHours += timeDiffHours;
                }
                else if (((h.StartTime >= regularStartTime) && (h.StartTime < regularEndTime)) || // Regular hours inside the range or overlapping the range 06 to 18
                          ((h.EndTime <= regularEndTime) && (h.EndTime >= regularStartTime)))
                {
                    var workingHoursStart = (h.StartTime >= regularStartTime) ? h.StartTime : regularStartTime;
                    var workingHoursEnd = (h.EndTime <= regularEndTime) ? h.EndTime : regularEndTime;

                    var tempRegularHours = new TimeSpan(workingHoursEnd.Ticks - workingHoursStart.Ticks).TotalHours.ToDecimal();
                    bool earlyEveningHoursCalculated = false;
                    var earlyEveningHours = 0.0m;

                    // If overtime limit has not been passed then there increase regular or evening compensation hours
                    if (!overTime)
                    {
                        // Hours started on early evening compensation between 00-06, then increase evening hours for later calculations
                        if (h.StartTime < regularStartTime)
                        {
                            earlyEveningHours = new TimeSpan(regularStartTime.Ticks - h.StartTime.Ticks).TotalHours.ToDecimal();
                            eveningHours += earlyEveningHours;
                            earlyEveningHoursCalculated = true;
                        }

                        regularHours += tempRegularHours;

                        // If eveninghours + regular hours passes the limit, then rest of the hours are added to overtime
                        if ((regularHours + eveningHours) >= overTimeLimit)
                        {
                            // Balance the regular and evening hours, set overtime flag
                            var maximumAllowedRegulars = overTimeLimit - eveningHours;
                            if (regularHours > maximumAllowedRegulars)
                            {
                                overtimeHours += (regularHours - maximumAllowedRegulars);
                                regularHours = maximumAllowedRegulars;
                            }
                            if (earlyEveningHoursCalculated)
                            {
                                tempRegularHours += eveningHours;
                            }
                            overTime = true;
                        }
                    }
                    else
                    {
                        overtimeHours += tempRegularHours;
                    }

                    // If hours passes the regular hours range then add exceeding hours to evening compensation hours or to overtime hours
                    var exceedingRangeHours = h.TotalHours - tempRegularHours;

                    if (!overTime)
                    {
                        // Add only to evening hours if EndTime passes the end of regular working hours
                        if (h.EndTime > regularEndTime)
                        {
                            eveningHours += exceedingRangeHours;
                        }

                        // Check if overtime limit has been passed and if so add overtime hours and balance the regular and evening hours
                        if ((regularHours + eveningHours) >= overTimeLimit)
                        {
                            var overtimeHoursInc = (regularHours + eveningHours) - overTimeLimit;
                            if (h.StartTime < regularStartTime) // Hours started on evening compensation between 00-06, then decrease regularhours, if not then decrease evening hours
                            {
                                regularHours -= overtimeHoursInc;
                            }
                            else
                            {
                                eveningHours -= overtimeHoursInc;
                            }
                            overtimeHours += overtimeHoursInc;
                            overTime = true;
                        }
                    }
                    else
                    {
                        overtimeHours += exceedingRangeHours;
                    }
                }
                // Calculate hours which only hits into evening compensation range and check if those passes the overtime limit and add rest of the hours as overtime
                else if ((h.StartTime >= regularEndTime) && (h.EndTime <= eveningEndTime))
                {
                    var timeDiffHours = new TimeSpan(h.EndTime.Ticks - h.StartTime.Ticks).TotalHours.ToDecimal();
                    if (!overTime)
                    {
                        eveningHours += timeDiffHours;
                        if ((regularHours + eveningHours) > 8.0m)
                        {
                            var overtimeHoursAddition = regularHours + eveningHours - overTimeLimit;
                            eveningHours -= overtimeHoursAddition;
                            overtimeHours += overtimeHoursAddition;
                            overTime = true;
                        }
                    }
                    else
                    {
                        overtimeHours += timeDiffHours;
                    }
                }
            }

            // Return amount of hour per type
            switch (type)
            {
                case HoursType.All:
                    result = totalHours;
                    break;

                case HoursType.EveningWork:
                    result = eveningHours;
                    break;

                case HoursType.Overtime:
                    result = overtimeHours;
                    break;

                case HoursType.Regular:
                    result = regularHours;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}