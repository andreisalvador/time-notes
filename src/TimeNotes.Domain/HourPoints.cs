using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeNotes.Core;
using TimeNotes.Core.Constants;
using TimeNotes.Core.DomainObjects.Interfaces;
using TimeNotes.Domain.Validators;

namespace TimeNotes.Domain
{
    public class HourPoints : Entity<HourPoints>, IAggregateRoot
    {
        private readonly List<TimeEntry> _timeEntries;

        public IReadOnlyCollection<TimeEntry> TimeEntries => _timeEntries.AsReadOnly();
        public DateTime Date { get; private set; }
        public TimeSpan ExtraTime { get; private set; }
        public TimeSpan MissingTime { get; private set; }
        public Guid UserId { get; private set; }

        public HourPoints(DateTime date, Guid userId)
        {
            Date = date;
            ExtraTime = TimeSpan.FromMinutes(0);
            MissingTime = TimeSpan.FromMinutes(0);
            UserId = userId;
            _timeEntries = new List<TimeEntry>(TimeNotesConstants.MAX_NUMBER_OF_HOUR_POINTS);
            Validate();
        }

        public void AddTimeEntry(TimeEntry timeEntry, HourPointConfigurations hourPointConfigurations)
        {
            try
            {
                timeEntry.Validate();
                timeEntry.AssociateHourPoints(Id);
                _timeEntries.Add(timeEntry);
                RecalculateTimes(hourPointConfigurations);
            }
            catch (ValidationException except)
            {
                throw new Exception(except.Message);
            }
        }

        public void RemoveTimeEntry(TimeEntry timeEntry, HourPointConfigurations hourPointConfigurations)
        {
            _timeEntries.Remove(timeEntry);
            RecalculateTimes(hourPointConfigurations);
        }

        public void CalculateExtraTime(HourPointConfigurations hourPointConfigurations)
        {
            ExtraTime = TimeSpan.MinValue;
        }

        public void CalculateMissingTime(HourPointConfigurations hourPointConfigurations)
        {
            MissingTime = TimeSpan.MinValue;
        }

        public bool HasTimeEntry(TimeEntry timeEntry)
            => _timeEntries.Any(t => t.DateHourPointed.Hour.Equals(timeEntry.DateHourPointed.Hour) && t.DateHourPointed.Minute.Equals(timeEntry.DateHourPointed.Minute));

        private void RecalculateTimes(HourPointConfigurations hourPointConfigurations)
        {
            CalculateExtraTime(hourPointConfigurations);
            CalculateMissingTime(hourPointConfigurations);
        }

        public override void Validate()
        {
            Validate(this, new HourPointsValidator());
        }
    }
}
