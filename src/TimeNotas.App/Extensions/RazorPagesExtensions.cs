using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TimeNotes.Core.Attributes;

namespace TimeNotas.App.Extensions
{
    public static class RazorPagesExtensions
    {
        public static IEnumerable<SelectListItem> GetSelectItensFromEnum<TEnum>(this RazorPage page) where TEnum : Enum
        {
            IEnumerable<EnumDescriptionAttribute> descriptions = GetEnumDescriptionAttribute<TEnum>();

            return descriptions?.Where(w => w != null).Select(desc => new SelectListItem(desc.Description, desc.Value));
        }

        public static string GetCurrentEnumDescription<TEnum>(this RazorPage page, string currentEnumValue) where TEnum : Enum
           => GetEnumDescriptionAttribute<TEnum>().Where(enumDesc => enumDesc != null && enumDesc.Value.Equals(currentEnumValue)).SingleOrDefault()?.Description;

        private static IEnumerable<EnumDescriptionAttribute> GetEnumDescriptionAttribute<TEnum>()
            => ((TypeInfo)typeof(TEnum)).DeclaredFields.Select(member => member.GetCustomAttribute<EnumDescriptionAttribute>());
    }
}
