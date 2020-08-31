using System;
using System.ComponentModel;

namespace TimeNotes.Core.Attributes
{
    
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDescriptionAttribute : DescriptionAttribute
    {
        public string Value { get; }

        public EnumDescriptionAttribute(string description, string value) : base(description)
        {
            Value = value;            
        }
    }
}
