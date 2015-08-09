using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Timers;

namespace MetalMynds.Utilities
{
    public class FileWatcher
    {

            protected FileSystemWatcher BaseFileSystemWatcher = new FileSystemWatcher();
            protected Timer BaseTimer = new Timer();
            protected String BaseFilename;

            public event EventHandler FileChanged;

            public FileWatcher()
            {
                BaseTimer.Elapsed += new System.Timers.ElapsedEventHandler(HandleElapsed);
                BaseTimer.Interval = 1000;
            }

            public String Filename 
            {
                get
                {
                    return BaseFilename;
                }
                set
                {
                    BaseFilename = value;

                    Setup(BaseFilename);

                }
            }

            public Boolean Enabled
            {
                get
                {
                    return BaseFileSystemWatcher.EnableRaisingEvents;
                }
                set
                {
                    BaseFileSystemWatcher.EnableRaisingEvents = value;
                }
            }

            protected virtual void Setup(String Filename)
            {

                if (BaseFileSystemWatcher != null)
                {
                    BaseFileSystemWatcher = new FileSystemWatcher();
                }

                BaseFileSystemWatcher.Path = Path.GetDirectoryName(Filename);
                BaseFileSystemWatcher.Filter = Path.GetFileName(Filename);
                //BaseFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                BaseFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite; // Updates Only
                BaseFileSystemWatcher.EnableRaisingEvents = true;
                BaseFileSystemWatcher.Changed += new FileSystemEventHandler(HandleWatcherChanged);
            }

            protected virtual void HandleWatcherChanged(object sender, FileSystemEventArgs e)
            {
                if (!BaseTimer.Enabled)
                    BaseTimer.Start();
            }

            protected virtual void HandleElapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                BaseTimer.Stop();
                if (FileChanged != null)
                    FileChanged(this, null);
            }


    }
}
