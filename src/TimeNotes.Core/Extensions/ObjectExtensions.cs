using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace TimeNotes.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsList(this Type obj)
        {
            if (obj == null) return false;
            return obj.IsGenericType &&
                   obj.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable));

        }

        public static object[] ToObjectArray(this object obj, string[] excludePropertiesWith)
        {
            var properties = obj.GetType()
                                .GetProperties()
                                .Where(w => !w.Name.StartsWith("Id", System.StringComparison.OrdinalIgnoreCase) && !w.Name.EndsWith("Id", System.StringComparison.OrdinalIgnoreCase) && !excludePropertiesWith.Contains(w.Name))
                                .ToList();

            object[] result = new object[properties.Count];
            

            for (int index = 0; index < properties.Count; index++)
            {
                if (properties[index].PropertyType.IsList())
                {
                    foreach (object item in (IEnumerable)properties[index].GetValue(obj))
                        result = result.Concat(item.ToObjectArray(excludePropertiesWith)).ToArray();
                }
                else
                    result[index] = properties[index].GetValue(obj);
            }

            return result;
        }
    }
}
