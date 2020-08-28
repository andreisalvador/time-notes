using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNotes.Domain.Data.Interfaces;

namespace TimeNotes.Domain.Services
{
    public class HourPointsServices
    {
        private readonly IHourPointsRepository _hourPointsRepository;
        private readonly IHourPointConfigurationsRepository _hourPointConfigurationsRepository;

        public HourPointsServices(IHourPointsRepository hourPointsRepository, IHourPointConfigurationsRepository hourPointConfigurationsRepository)
        {
            _hourPointsRepository = hourPointsRepository;
            _hourPointConfigurationsRepository = hourPointConfigurationsRepository;
        }

        public async Task AddTimeEntryToHourPoints(Guid userId, TimeEntry timeEntry)
        {
            HourPoints hourPoints = await _hourPointsRepository.GetHourPointsWithTimeEntriesByDateAndUser(timeEntry.DateHourPointed, userId);
            HourPointConfigurations configurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(userId);

            if (hourPoints is null)
            {
                hourPoints = new HourPoints(timeEntry.DateHourPointed.Date, userId);
                hourPoints.AddTimeEntry(timeEntry, configurations);
                _hourPointsRepository.AddHourPoints(hourPoints);
            }
            else
            {
                if (hourPoints.HasTimeEntry(timeEntry))
                    return;

                hourPoints.AddTimeEntry(timeEntry, configurations);
                _hourPointsRepository.UpdateHourPoints(hourPoints);
            }

            _hourPointsRepository.AddTimeEntry(timeEntry);
            await _hourPointsRepository.Commit();
        }
    }
}
