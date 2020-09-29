using System;
using Xunit;

namespace TimeNotes.Domain.UnitTests.Fixtures
{
    [CollectionDefinition(nameof(FixtureWrapper))]
    public class FixtureWrapperCollection : ICollectionFixture<FixtureWrapper> { }
    public class FixtureWrapper : IDisposable
    {
        private readonly Lazy<TimeEntryFixture> _timeEntryFixture;
        private readonly Lazy<HourPointConfigurationsFixture> _hourPointConfigurationsFixture;
        private readonly Lazy<HourPointsFixture> _hourPointsFixture;

        public TimeEntryFixture TimeEntryFixture => _timeEntryFixture.Value;
        public HourPointConfigurationsFixture HourPointConfigurationsFixture => _hourPointConfigurationsFixture.Value;
        public HourPointsFixture HourPointsFixture => _hourPointsFixture.Value;

        public FixtureWrapper()
        {
            _timeEntryFixture = new Lazy<TimeEntryFixture>();
            _hourPointConfigurationsFixture = new Lazy<HourPointConfigurationsFixture>();
            _hourPointsFixture = new Lazy<HourPointsFixture>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
