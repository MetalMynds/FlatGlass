using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MetalMynds.Utilities
{
    public class BrowserHelper
    {

        public static void ShowLink(String Url)
        {
            String command = "%ProgramFiles%\\Internet Explorer\\iexplore.exe";

            command = Environment.ExpandEnvironmentVariables(command);

            ProcessStartInfo startInfo = new ProcessStartInfo(command, Url);
            Process.Start(startInfo);

        }

    }
}
