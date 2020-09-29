using System;
using TimeNotes.Core;

namespace TimeNotes.Domain.UnitTests.Fixtures.Base
{
    public abstract class BaseFixture<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        public abstract TEntity CreateValid(params object[] @params);
        public abstract TEntity CreateInvalid(params object[] @params);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
