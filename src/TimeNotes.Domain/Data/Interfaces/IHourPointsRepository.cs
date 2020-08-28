using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeNotes.Core.Data;

namespace TimeNotes.Domain.Data.Interfaces
{
    public interface IHourPointsRepository : IRepository<HourPoints>
    {
        void AddHourPoints(HourPoints hourPoints);
        void RemoveHourPoints(HourPoints hourPoints);
        void UpdateHourPoints(HourPoints hourPoints);
        Task<HourPoints> GetHourPointsById(Guid hourPointsId);
        Task<IEnumerable<HourPoints>> GetAllHourPoints();
        Task<IEnumerable<HourPoints>> GetAllHourPointsWithTimeEntries(Guid userId);
        Task<IEnumerable<HourPoints>> GetHourPointsWhere(Func<HourPoints, bool> predicate);
        Task<HourPoints> GetHourPointsWithTimeEntriesByDateAndUser(DateTime date, Guid userId);

        void AddTimeEntry(TimeEntry timeEntry);
        void RemoveTimeEntry(TimeEntry timeEntry);
        void UpdateTimeEntry(TimeEntry timeEntry);
        Task<TimeEntry> GetTimeEntryById(Guid timeEntryId);
        Task<IEnumerable<TimeEntry>> GetAllTimeEntries();
        Task<IEnumerable<TimeEntry>> GetTimeEntriesWhere(Func<TimeEntry, bool> predicate);
    }
}
