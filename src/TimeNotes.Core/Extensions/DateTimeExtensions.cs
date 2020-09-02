using System;

namespace TimeNotes.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToBrazilianDateTime(this DateTime value)
        {   
            TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            if (brasiliaTimeZone?.StandardName == TimeZoneInfo.Local.StandardName)
                return value;

            return TimeZoneInfo.ConvertTimeFromUtc(value, brasiliaTimeZone);
        }
    }
}
