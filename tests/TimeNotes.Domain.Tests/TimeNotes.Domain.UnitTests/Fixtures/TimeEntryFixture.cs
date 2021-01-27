using System;
using TimeNotes.Domain.UnitTests.Fixtures.Base;

namespace TimeNotes.Domain.UnitTests.Fixtures
{
    public class TimeEntryFixture : BaseFixture<TimeEntry>
    {
        public override TimeEntry CreateInvalid(params object[] @params)
            => new TimeEntry(DateTime.MinValue);

        public override TimeEntry CreateValid(params object[] @params)
            => new TimeEntry(@params.Length <= 0 ? DateTime.Now.Date : (DateTime)@params[0]);
    }
}
