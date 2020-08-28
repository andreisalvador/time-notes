using System;
using System.ComponentModel.DataAnnotations;

namespace TimeNotas.App.Models
{
    public class TimeEntryModel
    {
        public TimeEntryModel()
        {
            DateHourPointed = DateTime.Now;
        }
        
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The date and hour is required")]
        [DataType(DataType.DateTime)]
        public DateTime DateHourPointed { get; set; }

        public override string ToString()
            => DateHourPointed.ToShortTimeString();
    }
}
