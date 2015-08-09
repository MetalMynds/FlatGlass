using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public class EnumHelper
    {

        public static TEnum Max<TEnum>()
            where TEnum : IComparable, IConvertible, IFormattable
        {
            Type type = typeof(TEnum);

            if (!type.IsSubclassOf(typeof(Enum)))
                throw new
                    InvalidCastException
                        ("Cannot cast '" + type.FullName + "' to System.Enum.");

            return (TEnum)Enum.ToObject(type, Enum.GetValues(type).Cast<int>().Last());
        }

        public static TEnum Min<TEnum>()
                    where TEnum : IComparable, IConvertible, IFormattable
        {
            Type type = typeof(TEnum);

            if (!type.IsSubclassOf(typeof(Enum)))
                throw new
                    InvalidCastException
                        ("Cannot cast '" + type.FullName + "' to System.Enum.");

            return (TEnum)Enum.ToObject(type, Enum.GetValues(type).Cast<int>().First());
        }
    }
}
