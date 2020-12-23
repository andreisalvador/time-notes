using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TimeNotes.Core.Enums;
using TimeNotes.Core.Attributes;

namespace TimeNotas.App.Models
{
    public class HourPointConfigurationsModel
    {
        public HourPointConfigurationsModel() { }

        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Work days")]
        public DaysOfWeek WorkDays { get; set; }

        [Display(Name = "Bank of hours type")]
        public BankOfHoursType BankOfHours { get; set; }

        [Display(Name = "Office hour")]
        [DataType(DataType.Time)]
        public TimeSpan OfficeHour { get; set; }

        [Display(Name = "Lunch time")]
        [DataType(DataType.Time)]
        public TimeSpan LunchTime { get; set; }

        [Display(Name = "Start work time")]
        [DataType(DataType.Time)]
        public TimeSpan StartWorkTime { get; set; }

        [Display(Name = "Tolerance time")]
        [DataType(DataType.Time)]        
        public TimeSpan ToleranceTime { get; set; }

        [Display(Name = "Hour value")]
        [DataType(DataType.Currency)]
        public decimal HourValue { get; set; }

        [Display(Name = "Use alexa support")]        
        public bool UseAlexaSupport { get; set; }
    }
}
