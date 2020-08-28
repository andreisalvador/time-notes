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

        public override void Validate()
        {
            Validate(this, new HourPointConfigurationsValidator());
        }
    }
}
