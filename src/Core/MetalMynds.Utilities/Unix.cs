using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public static class Unix
    {

        public static String Path(String Win32Path)
        {
            StringBuilder delimiter = new StringBuilder();
            
            delimiter.Append((char)92); // Add the \ Char

            if (Win32Path.Contains("\\")) {

                return Win32Path.Replace("\\","//");

            } else {

                return Win32Path.Replace(delimiter.ToString(), "//");

            }
        }

    }
}
