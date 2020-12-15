using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeNotes.Domain
{
    public class MonthExtract
    {
        private readonly IReadOnlyCollection<HourPoints> _hourPoints;

        public IReadOnlyCollection<HourPoints> HourPoints => _hourPoints;
        public DateTime Date { get; private set; }
        public TimeSpan ExtraTime { get; private set; }
        public TimeSpan MissingTime { get; private set; }
        public TimeSpan WorkedTime { get; private set; }
        public decimal PredictedSalary { get; private set; }

        public MonthExtract(DateTime date, IEnumerable<HourPoints> hourPoints, HourPointConfigurations currentConfiguration)
        {
            Date = date;
            _hourPoints = hourPoints.Where(h => h.Date.Month == date.Month && h.Date.Year == date.Year).ToList().AsReadOnly();

            CalculateExtraAndMissingTime();
            CalculateWorkedTime(currentConfiguration);
            CalculatePredictedSalary(currentConfiguration);
        }


        private void CalculateExtraAndMissingTime()
        {
            long extraTimeInTicks = _hourPoints.Sum(extra => extra.ExtraTime.Ticks);
            long missingTimeInTicks = _hourPoints.Sum(missing => missing.MissingTime.Ticks);
            ExtraTime = TimeSpan.Zero;
            MissingTime = TimeSpan.Zero;

            if ((extraTimeInTicks + missingTimeInTicks) != 0)
                if (extraTimeInTicks > missingTimeInTicks)
                    ExtraTime = TimeSpan.FromTicks(extraTimeInTicks - missingTimeInTicks);
                else
                    MissingTime = TimeSpan.FromTicks(missingTimeInTicks - extraTimeInTicks);

        }

        private void CalculateWorkedTime(HourPointConfigurations currentConfiguration)
        {
            WorkedTime = TimeSpan.Zero;

            if (_hourPoints.Any())
            {
                long totalWorkedTimeInTicks = _hourPoints.Count * currentConfiguration.OfficeHour.Ticks;
                WorkedTime = TimeSpan.FromTicks((totalWorkedTimeInTicks + ExtraTime.Ticks) - MissingTime.Ticks);
            }
        }

        private void CalculatePredictedSalary(HourPointConfigurations configurations)
        {
            decimal valueByMinute = configurations.HourValue / 60;

            PredictedSalary = (decimal)WorkedTime.TotalMinutes * valueByMinute;
        }
    }
}
