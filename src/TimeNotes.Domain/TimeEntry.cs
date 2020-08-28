using System;
using TimeNotes.Core;
using TimeNotes.Domain.Validators;

namespace TimeNotes.Domain
{
    public class TimeEntry : Entity<TimeEntry>
    {
        public DateTime DateHourPointed { get; private set; }
        public Guid HourPointsId { get; private set; }

        //EF core.
        public HourPoints HourPoints { get; set; }

        public TimeEntry(DateTime dateHourPointed)
        {
            DateHourPointed = dateHourPointed;
            Validate();
        }

        internal void AssociateHourPoints(Guid hourPointsId)
            => HourPointsId = hourPointsId;

        public override void Validate()
        {
            Validate(this, new TimeEntryValidator());
        }
    }
}
