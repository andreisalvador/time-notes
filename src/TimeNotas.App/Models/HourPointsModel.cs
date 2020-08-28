using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeNotas.App.Models
{
    public class HourPointsModel
    {
        public Guid Id { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Extra time")]
        public TimeSpan ExtraTime { get; set; }

        [DisplayName("Missing time")]
        public TimeSpan MissingTime { get; set; }

        public List<TimeEntryModel> TimeEntries { get; set; }

        public HourPointsModel(List<TimeEntryModel> timeEntryModels)
        {
            TimeEntries = timeEntryModels ?? new List<TimeEntryModel>();
        }
    }
}
