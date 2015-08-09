using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public class CompareHelper
    {

        public static bool Compare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }


    }
}
