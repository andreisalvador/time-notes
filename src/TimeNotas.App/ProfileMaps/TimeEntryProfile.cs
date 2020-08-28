using AutoMapper;
using System.Collections.Generic;
using TimeNotas.App.Models;
using TimeNotes.Domain;

namespace TimeNotas.App.ProfileMaps
{
    public class TimeEntryProfile : Profile
    {
        public TimeEntryProfile()
        {
            CreateMap<TimeEntryModel, TimeEntry>();
            CreateMap<TimeEntryModel, TimeEntry>().ReverseMap();
            CreateMap<List<TimeEntryModel>, List<TimeEntry>>();
            CreateMap<List<TimeEntryModel>, List<TimeEntry>>().ReverseMap();
        }
    }
}
