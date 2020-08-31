using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;

namespace TimeNotes.Data.Repository
{
    public class HourPointConfigurationsRepository : IHourPointConfigurationsRepository
    {
        private readonly TimeNotesContext _context;

        public HourPointConfigurationsRepository(TimeNotesContext context)
        {
            _context = context;
        }

        public void AddHourPointConfiguration(HourPointConfigurations hourPointConfigurations)
        {
            _context.HourPointConfigurations.Add(hourPointConfigurations);
        }

        public async Task<bool> Commit()
            => await _context.SaveChangesAsync() > 0;

        public async Task<HourPointConfigurations> GetHourPointConfigurationsById(Guid hourPointConfigurationsId)
            => await _context.HourPointConfigurations.SingleOrDefaultAsync(s => s.Id.Equals(hourPointConfigurationsId));


        public async Task<HourPointConfigurations> GetHourPointConfigurationsByUserId(Guid userId)
            => await _context.HourPointConfigurations.FirstOrDefaultAsync(f => f.UserId.Equals(userId));

        public void UpdateHourPointConfiguration(HourPointConfigurations hourPointConfigurations)
        {
            _context.HourPointConfigurations.Update(hourPointConfigurations);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
