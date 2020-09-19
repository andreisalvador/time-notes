using System;

namespace TimeNotes.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetDateTimeInFirstDate(this DateTime value) => value.AddDays(-(value.Date.Day - 1));
        public static DateTime GetDateTimeInLastDate(this DateTime value) => new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
    }
}
