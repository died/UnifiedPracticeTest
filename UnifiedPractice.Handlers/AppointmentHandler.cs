using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ical.Net.DataTypes;
using UnifiedPractice.Interfaces;
using UnifiedPractice.Models;
using UnifiedPractice.Providers;
using UnifiedPractice.Utility;

namespace UnifiedPractice.Handlers
{
    public class AppointmentHandler
    {
        private readonly ITestProvider _tp;

        public AppointmentHandler(ITestProvider tp)
        {
            _tp = tp;
        }

        /// <summary>
        /// Get Availability Time from date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<string> GetAvailabilityTime(string date)
        {
            //check input
            if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out var dt) &&
                !DateTime.TryParse(date, out dt)) return new List<string>();

            //get from cache
            var at = Caching.GetAvailabilityTime(dt);
            if (at != null) return at;

            //get time list from DB
            var queryResult = _tp.GetAvailabilityTime(dt).GetAwaiter().GetResult();
            if (queryResult == null) return new List<string>();

            //get Precedence first
            var min = queryResult.Min(x => x.Precedence);
            var list = queryResult.Where(x => x.Precedence == min).ToList();
            var result = ProcessAvailabilityTime(list, dt);

            //add to cache
            Caching.SetAvailabilityTime(date, result);

            return result;
        }

        /// <summary>
        /// Return possible time from given date
        /// </summary>
        /// <param name="list">List from db query result</param>
        /// <param name="dt">given date</param>
        /// <returns></returns>
        public List<string> ProcessAvailabilityTime(List<AvailabilityTime> list, DateTime dt)
        {
            //if no result
            if (list == null || !list.Any()) return new List<string>();

            var tzList = new List<TimeZone>();
            foreach (var at in list)
            {
                if (string.IsNullOrEmpty(at.Rrule)) continue;
                var rr = new RecurrencePattern(at.Rrule);

                LogUtility.Log(string.Join(',', rr.ByDay));

                var matchFlag = false;
                foreach (var wd in rr.ByDay)
                {
                    //not sure for the full requirement detail, so just check weekday only
                    if (wd.DayOfWeek == dt.DayOfWeek) matchFlag = true;
                }

                // get all match result as TimeZone list
                if (matchFlag) tzList.Add(new TimeZone { Start = at.StartTime, End = at.EndTime });
            }

            //no matched time
            if (!tzList.Any()) return new List<string>();

            //merge timezone
            var merged = MergeOverlapping(tzList);

            //convert to display string
            var result = new List<string>();
            foreach (var t in merged)
            {
                result.Add($"{t.Start:hh\\:mm}-{t.End:hh\\:mm}");
            }

            //TODO
            // requested response format [HH:mm-HH:mm] -> no " "
            // but i think it should have it, so I leave it as List<string> not just a format string

            return result;
        }

        #region MergeOverlapping
        /// <summary>
        /// Merge overlap timezone
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IEnumerable<TimeZone> MergeOverlapping(IEnumerable<TimeZone> list)
        {
            var accumulator = list.First();
            list = list.Skip(1);

            foreach (var interval in list)
            {
                if (interval.Start <= accumulator.End)
                {
                    accumulator = Combine(accumulator, interval);
                }
                else
                {
                    yield return accumulator;
                    accumulator = interval;
                }
            }

            yield return accumulator;
        }

        private TimeZone Combine(TimeZone start, TimeZone end)
        {
            return new TimeZone
            {
                Start = start.Start,
                End = Max(start.End, end.End),
            };
        }

        private static TimeSpan Max(TimeSpan left, TimeSpan right)
        {
            return (left > right) ? left : right;
        }
        #endregion
    }

    public class TimeZone
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}