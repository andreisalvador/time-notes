﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;
using TimeNotes.Infrastructure.Data;

namespace TimeNotes.Data.Repository
{
    public class HourPointsRepository : IHourPointsRepository
    {
        private readonly TimeNotesContext _context;

        public HourPointsRepository(TimeNotesContext context)
        {
            _context = context;
        }

        public void AddHourPoints(HourPoints hourPoints)
        {
            _context.HourPoints.Add(hourPoints);
        }

        public void AddTimeEntry(TimeEntry timeEntry)
        {
            _context.TimeEntries.Add(timeEntry);
        }

        public async Task<bool> Commit()
            => await _context.SaveChangesAsync() > 0;

        public async Task<IEnumerable<HourPoints>> GetAllHourPoints()
            => await _context.HourPoints.ToListAsync();

        public async Task<IEnumerable<HourPoints>> GetAllHourPointsWithTimeEntries(Guid userId)
            => await _context.HourPoints.Where(w => w.UserId.Equals(userId)).Include(t => t.TimeEntries).ToListAsync();

        public async Task<IEnumerable<TimeEntry>> GetAllTimeEntries()
            => await _context.TimeEntries.ToListAsync();

        public async Task<HourPoints> GetHourPointsById(Guid hourPointsId)
            => await _context.HourPoints.Include(t => t.TimeEntries).SingleOrDefaultAsync(s => s.Id.Equals(hourPointsId));

        public async Task<IEnumerable<HourPoints>> GetHourPointsWhere(Expression<Func<HourPoints, bool>> predicate)
            => await Task.FromResult(_context.HourPoints.Where(predicate).Include(h => h.TimeEntries));

        public async Task<IEnumerable<TimeEntry>> GetTimeEntriesWhere(Expression<Func<TimeEntry, bool>> predicate)
            => await Task.FromResult(_context.TimeEntries.Where(predicate).Include(t => t.HourPoints));

        public async Task<TimeEntry> GetTimeEntryById(Guid timeEntryId)
            => await _context.TimeEntries.SingleOrDefaultAsync(s => s.Id.Equals(timeEntryId));

        public void RemoveHourPoints(HourPoints hourPoints)
        {
            _context.HourPoints.Remove(hourPoints);
        }

        public void RemoveTimeEntry(TimeEntry timeEntry)
        {
            _context.TimeEntries.Remove(timeEntry);
        }
        public void UpdateHourPoints(HourPoints hourPoints)
        {
            _context.HourPoints.Update(hourPoints);
        }

        public void UpdateTimeEntry(TimeEntry timeEntry)
        {
            _context.TimeEntries.Update(timeEntry);
        }

        public async Task<HourPoints> GetHourPointsWithTimeEntriesByDateAndUser(DateTime date, Guid userId)
            => await _context.HourPoints.Where(w => w.Date.Date.Equals(date.Date) && w.UserId.Equals(userId)).Include(t => t.TimeEntries).SingleOrDefaultAsync();

        public async Task<bool> ExistsHourPointsToDateAndUser(DateTime date, Guid userId)
            => await _context.HourPoints.Where(w => w.Date.Date.Equals(date.Date) && w.UserId.Equals(userId)).AnyAsync();

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
