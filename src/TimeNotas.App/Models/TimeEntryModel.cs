using System;
using System.ComponentModel.DataAnnotations;

namespace TimeNotas.App.Models
{
    public class TimeEntryModel
    {
        public Guid Id { get; set; }
        public Guid HourPointsId { get; set; }

        [Required(ErrorMessage = "The date and hour is required")]
        [DataType(DataType.DateTime)]
        public DateTime DateHourPointed { get; set; } = DateTime.Now.ToLocalTime();

        public override string ToString()
            => DateHourPointed.ToShortTimeString();
    }
}
