using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DriverAssist.Common
{
    public static class EnumX
    {
        public static IEnumerable<TEnum> FilterByAttribute<TEnum, TAttribute>()
            where TEnum : struct
            where TAttribute : Attribute
        {
            return from field in typeof(TEnum).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
                   where field.GetCustomAttributes(typeof(TAttribute), false).Length > 0
                   select (TEnum)field.GetValue(null);
        }
    }
}
