using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace MLCS.Common.Utils
{
    public static class Extensions
    {
        // https://stackoverflow.com/questions/1415140/can-my-enums-have-friendly-names/1415187#1415187
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }

        public static bool IsNotEmpty(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return false;

            IEnumerator enumerator = enumerable.GetEnumerator();
            return enumerator.MoveNext();
        }
    }
}