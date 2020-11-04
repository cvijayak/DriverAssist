using System;

namespace DriverAssist.Common
{
    public static class StringX
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
