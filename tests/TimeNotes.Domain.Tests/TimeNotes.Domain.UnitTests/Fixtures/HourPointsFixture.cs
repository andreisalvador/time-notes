using System;
using TimeNotes.Domain.UnitTests.Fixtures.Base;

namespace TimeNotes.Domain.UnitTests.Fixtures
{
    public class HourPointsFixture : BaseFixture<HourPoints>
    {
        public override HourPoints CreateInvalid(params object[] @params)
            => new HourPoints(DateTime.MinValue, Guid.Empty);
            
        public override HourPoints CreateValid(params object[] @params)
            => new HourPoints(DateTime.Now, Guid.NewGuid());
    }
}
