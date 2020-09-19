using System;
using TimeNotes.Core;
using TimeNotes.Domain.Validators;

namespace TimeNotes.Domain
{
    public class TimeEntry : Entity<TimeEntry>
    {
        private DateTime _dateHourPointed;

        public DateTime DateHourPointed { get => _dateHourPointed; private set { _dateHourPointed = value.AddSeconds(-value.Second); } }
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

        public void ChangeDateHourPointed(DateTime dateHourPointed)
        {
            if (dateHourPointed.Date != DateHourPointed.Date)
                throw new ArgumentException("The hour's date points can't be different.");

            if (SameTime(dateHourPointed))
                return;

            DateHourPointed = dateHourPointed;
        }

        public bool SameTime(DateTime dateHourPointed)
            => dateHourPointed.Hour.Equals(DateHourPointed.Hour) && dateHourPointed.Minute.Equals(DateHourPointed.Minute);

        public override void Validate()
        {
            Validate(this, new TimeEntryValidator());
        }
    }
}
