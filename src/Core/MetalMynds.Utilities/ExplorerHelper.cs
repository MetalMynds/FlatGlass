using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MetalMynds.Utilities
{
    public class ExplorerHelper {
            

        public static void ShowFolder(String Path)
        {
            if (Directory.Exists(Path))
            {
                String command = "%Systemroot%\\explorer.exe";

                command = Environment.ExpandEnvironmentVariables(command);

                ProcessStartInfo startInfo = new ProcessStartInfo(command, Path);
                Process.Start(startInfo);
            }
            else
            {
                throw new DirectoryNotFoundException("Unable to Find Folder [" + Path + "] to Open.");
            }
        }        

        

    }
}
