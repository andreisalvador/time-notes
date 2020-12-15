using FluentAssertions;
using FluentValidation;
using TimeNotes.Domain.UnitTests.Fixtures;
using Xunit;

namespace TimeNotes.Domain.UnitTests
{
    [Collection(nameof(FixtureWrapper))]
    public class HourPointsTests
    {
        private readonly FixtureWrapper _fixtureWrapper;

        public HourPointsTests(FixtureWrapper fixtureWrapper)
        {
            _fixtureWrapper = fixtureWrapper;
        }

        [Fact]
        public void HourPoints_CreateValid_ShouldReturnValidHourPoints()
        {
            _fixtureWrapper.TimeEntryFixture.Invoking(t => t.CreateValid()).Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void HourPoints_CreateInvalid_ShouldReturnErrorOnCreatingHourPoints()
        {
            _fixtureWrapper.TimeEntryFixture.Invoking(t => t.CreateInvalid()).Should().Throw<ValidationException>();
        }
    }
}
