﻿using Microsoft.AspNetCore.Mvc.Rendering;
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
        public Guid Id { get; set; }

        [Display(Name = "Work days")]
        public DaysOfWeek WorkDays { get; set; }

        [Display(Name = "Office hour")]
        public TimeSpan OfficeHour { get; set; }

        [Display(Name = "Lunch time")]
        public TimeSpan LunchTime { get; set; }

        public IEnumerable<SelectListItem> GetWorkDaysDescriptions()
        {
            IEnumerable<EnumDescriptionAttribute> descriptions = GetWorkDaysEnumDescriptionAttributes();

            return descriptions?.Where(w => w != null).Select(desc => new SelectListItem(desc.Description, desc.Value));
        }

        public string GetCurrentWorkDaysDescription()
            => GetWorkDaysEnumDescriptionAttributes().Where(enumDesc => enumDesc != null && enumDesc.Value.Equals(((int)WorkDays).ToString())).SingleOrDefault()?.Description;

        private IEnumerable<EnumDescriptionAttribute> GetWorkDaysEnumDescriptionAttributes()
            => ((TypeInfo)typeof(DaysOfWeek)).DeclaredFields.Select(member => member.GetCustomAttribute<EnumDescriptionAttribute>());
    }
}