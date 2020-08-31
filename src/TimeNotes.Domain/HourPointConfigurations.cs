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
        public TimeSpan OfficeHour { get; private set; }
        public TimeSpan LunchTime { get; private set; }
        public Guid UserId { get; private set; }

        
        public HourPointConfigurations(DaysOfWeek workDays, TimeSpan officeHour, TimeSpan lunchTime, Guid userId)
        {
            WorkDays = workDays;
            OfficeHour = officeHour;
            LunchTime = lunchTime;
            UserId = userId;
            Validate();
        }

        public void ChangeWorkDays(DaysOfWeek workDays)
        {
            if (Enum.IsDefined(typeof(DaysOfWeek), workDays))
                WorkDays = workDays;
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

        public override void Validate()
        {
            Validate(this, new HourPointConfigurationsValidator());
        }

        private bool IsTimeValid(TimeSpan time)
            => time > TimeSpan.MinValue && (time.Hours <= 23 && time.Minutes <= 59);
    }
}
