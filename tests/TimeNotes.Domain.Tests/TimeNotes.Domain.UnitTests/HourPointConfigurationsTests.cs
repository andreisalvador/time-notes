using FluentAssertions;
using FluentValidation;
using TimeNotes.Domain.UnitTests.Fixtures;
using Xunit;

namespace TimeNotes.Domain.UnitTests
{
    [Collection(nameof(FixtureWrapper))]
    public class HourPointConfigurationsTests
    {
        private readonly FixtureWrapper _fixtureWrapper;

        public HourPointConfigurationsTests(FixtureWrapper fixtureWrapper)
        {
            _fixtureWrapper = fixtureWrapper;
        }

        [Fact]
        public void HourPointConfigurations_CreateValid_ShouldReturnValidHourPointConfigurations()
        {
            _fixtureWrapper.HourPointConfigurationsFixture.Invoking(t => t.CreateValid()).Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void HourPointConfigurations_CreateInvalid_ShouldReturnErrorOnCreatingHourPointConfigurations()
        {
            _fixtureWrapper.HourPointConfigurationsFixture.Invoking(t => t.CreateInvalid()).Should().Throw<ValidationException>();
        }
    }
}
