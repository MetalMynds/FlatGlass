using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MetalMynds.Utilities
{
    public class JavaPropertiesHelper
    {

        public static Dictionary<String, String> GetProperties(String Filename)
        {
            Dictionary<String, String> properties = new Dictionary<string, string>();

            String[] lines = File.ReadAllLines(Filename);

            foreach (String line in lines)
            {

                if ((!line.TrimStart().StartsWith("#")) && (line.Trim() != String.Empty)) {

                    String[] property = line.Split('=');

                    if (property.Length > 1)
                    {
                        properties.Add(property[0].Trim(), property[1].Trim());
                    }

                }

            }

            return properties;

        }

    }
}
