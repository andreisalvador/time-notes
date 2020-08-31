using System;
using System.Threading.Tasks;
using TimeNotes.Core.Data;

namespace TimeNotes.Domain.Data.Interfaces
{
    public interface IHourPointConfigurationsRepository : IRepository<HourPointConfigurations>
    {
        void AddHourPointConfiguration(HourPointConfigurations hourPointConfigurations);
        void UpdateHourPointConfiguration(HourPointConfigurations hourPointConfigurations);
        Task<HourPointConfigurations> GetHourPointConfigurationsById(Guid hourPointConfigurationsId);
        Task<HourPointConfigurations> GetHourPointConfigurationsByUserId(Guid userId);
    }
}
