using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MetalMynds.Utilities
{
    public class TailHelper
    {

        public static Boolean TryTail(String Filename, int LineCount, out int LinesTailed,out List<String> Tailed, out String Error)
        {

            try
            {


                FileStream stream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new System.IO.StreamReader(stream);

                String[] lines;

                lines = Regex.Split(reader.ReadToEnd(), "\r\n");

                //lines = File.ReadAllLines(Filename);

                if (lines.Count() < LineCount)
                {
                    Tailed = new List<string>(lines);
                }
                else
                {
                    Tailed = new List<string>();

                    for (int count = (lines.Count() - LineCount);count<lines.Count()-1;count++) {

                        Tailed.Add(lines[count]);

                    }
                }

                Error = String.Empty;
                LinesTailed = Tailed.Count;
                return true;

            }
            catch (Exception ex)
            {
                Error = String.Format("Failed Trying to Tail File!\nFilename: {0}\nError: {1}\nStackTrace: {2}", Filename, ex.Message, ex.StackTrace);
                LinesTailed = 0;
                Tailed = new List<string>();
                return false;
            }

        }

    }
}
