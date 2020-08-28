using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeNotas.App.Models;
using TimeNotes.Domain;

namespace TimeNotas.App.ProfileMaps
{
    public class HourPointsProfile : Profile
    {
        public HourPointsProfile()
        {
            CreateMap<HourPoints, HourPointsModel>();
            CreateMap<HourPoints, HourPointsModel>().ReverseMap();
            CreateMap<IEnumerable<HourPoints>, List<HourPointsModel>>();
            CreateMap<IEnumerable<HourPoints>, List<HourPointsModel>>().ReverseMap();
        }
    }
}
