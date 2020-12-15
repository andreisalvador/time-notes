using System;
using TimeNotes.Core;
using TimeNotes.Core.DomainObjects.Interfaces;
using TimeNotes.Core.Enums;
using TimeNotes.Domain.Validators;

namespace TimeNotes.Domain
{
    public class HourPointConfigurations : Entity<HourPointConfigurations>, IAggregateRoot
    {
        public DaysOfWeek WorkDays { get; private set; }
        public BankOfHoursType BankOfHours { get; private set; }
        public TimeSpan OfficeHour { get; private set; }
        public TimeSpan LunchTime { get; private set; }
        public TimeSpan ToleranceTime { get; private set; }
        public TimeSpan StartWorkTime { get; private set; }
        public decimal HourValue { get; private set; }
        public Guid UserId { get; private set; }


        public HourPointConfigurations(DaysOfWeek workDays, BankOfHoursType bankOfHours, TimeSpan officeHour, TimeSpan lunchTime, TimeSpan startWorkTime, TimeSpan toleranceTime, Guid userId, decimal hourValue = 0m)
        {
            WorkDays = workDays;
            BankOfHours = bankOfHours;
            OfficeHour = officeHour;
            LunchTime = lunchTime;
            StartWorkTime = startWorkTime;
            ToleranceTime = toleranceTime;
            UserId = userId;
            HourValue = hourValue;
            Validate();
        }

        public void ChangeWorkDays(DaysOfWeek workDays)
        {
            if (Enum.IsDefined(typeof(DaysOfWeek), workDays))
                WorkDays = workDays;
        }

        public void ChangeBankOfHours(BankOfHoursType bankOfHoursType)
        {
            if (Enum.IsDefined(typeof(BankOfHoursType), bankOfHoursType))
                BankOfHours = bankOfHoursType;
        }

        public void ChangeLunchTime(TimeSpan lunchTime)
        {
            if (IsTimeValid(lunchTime))
                LunchTime = lunchTime;
        }

        public void ChangeOfficeHour(TimeSpan officeHour)
        {
            if (IsTimeValid(officeHour))
                OfficeHour = officeHour;
        }

        public void ChangeStartWorkTime(TimeSpan startWorkTime)
        {
            if (IsTimeValid(startWorkTime))
                StartWorkTime = startWorkTime;
        }

        public void ChangeToleranceTime(TimeSpan toleranceTime)
        {
            if (IsTimeValid(toleranceTime))
                ToleranceTime = toleranceTime;
        }

        public void ChangeHourValue(decimal newHourValue)
        {
            if (newHourValue > 0 && newHourValue != HourValue)
                HourValue = newHourValue;
        }

        public override void Validate()
        {
            Validate(this, new HourPointConfigurationsValidator());
        }


        private bool IsTimeValid(TimeSpan time)
            => time > TimeSpan.MinValue && (time.Hours <= 23 && time.Minutes <= 59);
    }
}
