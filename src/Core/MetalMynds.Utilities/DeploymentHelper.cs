using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MetalMynds.Utilities
{
    public class DeploymentHelper
    {

        public static void Uninstall(String ProductCode, Boolean Passive, Boolean Wait)
        {
            string args = String.Empty;

            if (Passive)
            {
                args = String.Format("/passive /x {0}", ProductCode);
            }
            else
            {
                args = String.Format("/x {0}", ProductCode);
            }

            Process msiExecProcess = Process.Start("msiexec.exe", args);

            if (Wait) msiExecProcess.WaitForExit();

        }

        public static void Install(String MSIPath, Boolean Passive, Boolean Wait)
        {
            string args = String.Empty;

            if (Passive)
            {
                args = String.Format("/passive /i \"{0}\"", MSIPath);
            }
            else
            {
                args = String.Format("/i \"{0}\"", MSIPath);
            }

            Process msiExecProcess = Process.Start("msiexec.exe", args);

            if (Wait) msiExecProcess.WaitForExit();

        }

    }
}
