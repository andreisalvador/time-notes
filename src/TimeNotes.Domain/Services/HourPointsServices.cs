﻿using System;
using System.Linq;
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

        public async Task RemoveTimeEntryFromHourPoints(Guid userId, Guid timeEntryId)
        {
            TimeEntry timeEntry = await _hourPointsRepository.GetTimeEntryById(timeEntryId);
            HourPoints hourPoints = await _hourPointsRepository.GetHourPointsById(timeEntry.HourPointsId);
            HourPointConfigurations hourPointConfigurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(userId);

            hourPoints.RemoveTimeEntry(timeEntry, hourPointConfigurations);

            _hourPointsRepository.RemoveTimeEntry(timeEntry);

            if (!hourPoints.TimeEntries.Any())
                _hourPointsRepository.RemoveHourPoints(hourPoints);
            else
                _hourPointsRepository.UpdateHourPoints(hourPoints);

            await _hourPointsRepository.Commit();
        }
    }
}