using System;

namespace TimeNotes.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToFormattedStringTime(this TimeSpan value)
            => $"{value.Hours}h {value.Minutes}m";
    }
}
