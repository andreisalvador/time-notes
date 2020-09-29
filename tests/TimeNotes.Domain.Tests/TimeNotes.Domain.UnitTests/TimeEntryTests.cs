using FluentAssertions;
using FluentValidation;
using System;
using TimeNotes.Domain.UnitTests.Fixtures;
using Xunit;

namespace TimeNotes.Domain.UnitTests
{
    [Collection(nameof(FixtureWrapper))]
    public class TimeEntryTests
    {
        private readonly FixtureWrapper _fixtureWrapper;

        public TimeEntryTests(FixtureWrapper fixtureWrapper)
        {
            _fixtureWrapper = fixtureWrapper;
        }
        
        [Fact]
        public void TimeEntry_CreateValid_ShouldReturnValidTimeEntry()
        {
            _fixtureWrapper.TimeEntryFixture.Invoking(t => t.CreateValid()).Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void TimeEntry_CreateInvalid_ShouldReturnErrorOnCreatingTimeEntry()
        {
            _fixtureWrapper.TimeEntryFixture.Invoking(t => t.CreateInvalid()).Should().Throw<ValidationException>();
        }

        [Fact]
        public void TimeEntry_SameTime_ShouldReturnTrueBecauseDateTimeIsTheSameFromDateHourPointed()
        {
            TimeEntry timeEntry = _fixtureWrapper.TimeEntryFixture.CreateValid(DateTime.Now);
            timeEntry.SameTime(DateTime.Now).Should().BeTrue();
        }

        [Fact]
        public void TimeEntry_SameTime_ShouldReturnFalseBecauseDateTimeIsNotTheSameFromDateHourPointed()
        {
            TimeEntry timeEntry = _fixtureWrapper.TimeEntryFixture.CreateValid(DateTime.Now.AddHours(1).AddMinutes(1));
            timeEntry.SameTime(DateTime.Now).Should().BeFalse();
        }

        [Fact]
        public void TimeEntry_ChangeDateHourPointed_ShouldChangeTheDateHourPointedFromTimeEntry()
        {
            TimeEntry timeEntry = _fixtureWrapper.TimeEntryFixture.CreateValid();
            DateTime newDateHourPointed = DateTime.Now.AddHours(2).AddSeconds(-DateTime.Now.Second);
            timeEntry.ChangeDateHourPointed(newDateHourPointed);

            timeEntry.DateHourPointed.Should().Be(newDateHourPointed);
        }

        [Fact]
        public void TimeEntry_ChangeDateHourPointed_ShouldThrowErrorBecauseTheDateIsDifferent()
        {
            TimeEntry timeEntry = _fixtureWrapper.TimeEntryFixture.CreateValid();
            DateTime newDateHourPointed = DateTime.Now.AddDays(1);
            timeEntry.Invoking(t => t.ChangeDateHourPointed(newDateHourPointed)).Should().Throw<ArgumentException>();
        }
    }
}
