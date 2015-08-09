// -----------------------------------------------------------------------
// <copyright file="ClickOnceHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Diagnostics;
    using System.IO;
    using System.Management;
    using System.Threading;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClickOnceHelper
    {

        public static Process LaunchFromMenu(String ItemPath, String ProcessName)
        {

            bool isRunning = true;
            
            Process[] existingProcesses = Process.GetProcessesByName(ProcessName);

            foreach (Process existingProcess in existingProcesses)
            {
                try
                {
                    existingProcess.Kill();
                    existingProcess.WaitForExit();
                }
                catch (Exception ex)
                {

                }
            }

            String fullPath = String.Format("{0}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\{1}.appref-ms", System.Environment.ExpandEnvironmentVariables("%userprofile%"), ItemPath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("Menu Link File Not Found!", fullPath);
            }

            Process clickLauncherProcess = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo(fullPath);

            clickLauncherProcess.StartInfo = startInfo;

            clickLauncherProcess.Start();

            Boolean ready = false;

            Process runningProcess = null;

            while (!ready)
            {
                Process[] processList = Process.GetProcessesByName(ProcessName);

                foreach (Process clientProcess in processList)
                {

                    if (!clientProcess.HasExited)
                    {
                        ready = true;
                        runningProcess = clientProcess;
                        break;
                    }

                }
            }

            return runningProcess;

        }

    }
}
