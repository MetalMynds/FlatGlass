using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;

namespace MetalMynds.Utilities
{
    public static class IdentificationHelper
    {

        public static String FingerPrint(Assembly Binary)
        {
            return CryptographicHelper.CalculateMD5(Binary);
        }


    }
}
