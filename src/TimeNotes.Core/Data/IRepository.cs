using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeNotes.Core.DomainObjects.Interfaces;

namespace TimeNotes.Core.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
    {
        Task<bool> Commit();
    }
}
