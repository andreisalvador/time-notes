using System;
using TimeNotes.Core.Enums;
using TimeNotes.Domain.UnitTests.Fixtures.Base;

namespace TimeNotes.Domain.UnitTests.Fixtures
{
    public class HourPointConfigurationsFixture : BaseFixture<HourPointConfigurations>
    {
        public override HourPointConfigurations CreateInvalid(params object[] @params)
            => new HourPointConfigurations((DaysOfWeek)1231231, (BankOfHoursType)123123, TimeSpan.MinValue, TimeSpan.MinValue, TimeSpan.MinValue, TimeSpan.MinValue, Guid.Empty);

        public override HourPointConfigurations CreateValid(params object[] @params)
            => new HourPointConfigurations(DaysOfWeek.WorkingDay, BankOfHoursType.Monthly, TimeSpan.FromHours(8), TimeSpan.FromHours(1), TimeSpan.FromHours(9), TimeSpan.FromMinutes(5), Guid.NewGuid());
    }
}
