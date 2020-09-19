using System;
using System.Collections.Generic;
using System.Linq;
using TimeNotes.Domain;

namespace TimeNotas.App.Models
{
    public struct HomeDashboardViewModel
    {
        private TimeSpan _totalExtraTime;
        private TimeSpan _totalMissingTime;

        public string MonthName { get; set; }
        public string MonthNameWithYear { get; private set; }
        public TimeSpan TotalExtraTimeInMonth
        {
            get
            {
                if (_totalExtraTime.Ticks > _totalMissingTime.Ticks)
                {
                    _totalExtraTime = TimeSpan.FromTicks(_totalExtraTime.Ticks - _totalMissingTime.Ticks);
                    _totalMissingTime = TimeSpan.Zero;
                }

                return _totalExtraTime;
            }
            set { _totalExtraTime = value; }
        }
        public TimeSpan TotalMissingTimeInMonth
        {
            get
            {
                if (_totalMissingTime.Ticks > _totalExtraTime.Ticks)
                {
                    _totalMissingTime = TimeSpan.FromTicks(_totalMissingTime.Ticks - _totalExtraTime.Ticks);
                    _totalExtraTime = TimeSpan.Zero;
                }

                return _totalMissingTime;
            }
            set { _totalMissingTime = value; }
        }
        public TimeSpan TotalWorkedTimeInMonth { get; }

        public HomeDashboardViewModel(DateTime date, IEnumerable<HourPoints> userHourPoints, HourPointConfigurations hourPointConfigurations)
        {
            MonthName = new DateTime(date.Year, date.Month, 1).ToString("MMMM");
            MonthNameWithYear = $"{MonthName}/{date.Year}";
            _totalExtraTime = TimeSpan.FromTicks(userHourPoints.Sum(extra => extra.ExtraTime.Ticks));
            _totalMissingTime = TimeSpan.FromTicks(userHourPoints.Sum(missing => missing.MissingTime.Ticks));
            TotalWorkedTimeInMonth = userHourPoints.Any() ? TimeSpan.FromTicks(((userHourPoints.Count() * hourPointConfigurations.OfficeHour.Ticks) +
                                            _totalExtraTime.Ticks) - _totalMissingTime.Ticks) : TimeSpan.Zero;
        }
    }
}
