using System;
using TimeNotes.Core.Attributes;

namespace TimeNotes.Core.Enums
{
    [Flags]
    public enum DaysOfWeek
    {
        [EnumDescription("Working day", "1")]
        WorkingDay = 1,
        [EnumDescription("Weekend", "2")]
        Weekend = 2,
        [EnumDescription("All days", "4")]
        AllDays = 4
    }
}
