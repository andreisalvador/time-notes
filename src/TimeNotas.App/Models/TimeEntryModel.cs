using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using TimeNotes.Core.Extensions;

namespace TimeNotas.App.Models
{
    public class TimeEntryModel
    {
        public TimeEntryModel() { }

        [Key]
        public Guid Id { get; set; }

        [HiddenInput]
        public Guid HourPointsId { get; set; }

        [Required(ErrorMessage = "The date and hour is required")]
        [DataType(DataType.DateTime)]
        public DateTime DateHourPointed { get; set; } = DateTime.Now;

        public override string ToString()
            => DateHourPointed.ToShortTimeString();
    }
}
