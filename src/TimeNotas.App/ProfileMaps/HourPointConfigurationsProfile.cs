using AutoMapper;
using TimeNotas.App.Models;
using TimeNotes.Domain;

namespace TimeNotas.App.ProfileMaps
{
    public class HourPointConfigurationsProfile : Profile
    {
        public HourPointConfigurationsProfile()
        {
            CreateMap<HourPointConfigurations, HourPointConfigurationsModel>();
            CreateMap<HourPointConfigurations, HourPointConfigurationsModel>().ReverseMap();
        }
    }
}
