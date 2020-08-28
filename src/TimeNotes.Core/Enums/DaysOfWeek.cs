using System;
using System.ComponentModel;

namespace TimeNotes.Core.Enums
{
    [Flags]
    public enum DaysOfWeek
    {
        [Description("Working day")]
        WorkingDay = 1,
        [Description("Weekend")]
        Weekend,
        [Description("All days")]
        AllDay = WorkingDay + Weekend
    }
}
