using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MetalMynds.Utilities
{
    public class IOHelper
    {

        public static void WaitForFile(String Filename, int TimeoutSeconds)
        {
            //Console.WriteLine(String.Format("WaitForFile: [{0}]", Filename));
            
            const int SLEEP = 500;
            int loopCount = (TimeoutSeconds * 1000) / SLEEP;
            Boolean notAvailable = true;
            int count = 0;

            while (notAvailable && count < loopCount)
            {

                FileStream fs;

                try
                {

                    fs = File.OpenRead(Filename);
                    fs.Close();
                    notAvailable = false;

                }
                catch (FileNotFoundException ex)
                {
                    throw ex;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(500);
                }

                count +=1;

            }

            if (count == loopCount)
            {
                throw new IOException(String.Format("File [{0}] Is Locked By Another Process!", Filename));
            }

        }

        public static void WaitForFolder(String Folder, int TimeoutSeconds)
        {
            //Console.WriteLine(String.Format("WaitForFile: [{0}]", Filename));

            const int SLEEP = 500;
            int loopCount = (TimeoutSeconds * 1000) / SLEEP;
            Boolean available = true;
            int count = 0;

            while (!available && count < loopCount)
            {

                //FileStream fs;

                try
                {
                    available = Directory.Exists(Folder);
                }
                catch (FileNotFoundException ex)
                {
                    throw ex;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(500);
                }

                count += 1;

            }

            if (count == loopCount)
            {
                throw new IOException(String.Format("Folder [{0}] Is Unavailabe!", Folder));
            }

        }

    }
}
