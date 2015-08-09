using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public class Collections
    {

        public static List<String> RemoveDuplicates(List<String> Source)
        {
            List<String> unique = new List<string>();

            foreach (String value in Source)
            {
                if (!unique.Contains(value))
                {
                    unique.Add(value);
                }
            }

            return unique;
        }


    }
}
