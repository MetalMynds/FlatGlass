using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public static class ColorHelper
    {
        public static int ConvertFromHex(String Value)
        {
            return int.Parse(Value, System.Globalization.NumberStyles.HexNumber);
        }

        public static String ConvertToHex(int Value)
        {
            return Value.ToString("X").Substring(0, 6);
        }
    }
}