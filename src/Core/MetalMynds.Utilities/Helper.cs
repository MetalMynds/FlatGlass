using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MetalMynds.Utilities
{
    public interface IInfoWriter
    {
        void WriteInfoLine();
        void WriteInfoLine(string infoToWrite);
        void WriteInfoLine(string format, params string[] args);
    }
    public class DummyInfoWriter : IInfoWriter
    {
        #region IInfoWriter Members

        public void WriteInfoLine()
        {
        }
        public void WriteInfoLine(string infoToWrite)
        {
        }
        public void WriteInfoLine(string format, params string[] args)
        { 
            //  A formatted string similar to string.Format()
        }
        #endregion
    }
    public class ConsoleInfoWriter : IInfoWriter
    {
        #region IInfoWriter Members

        public void WriteInfoLine()
        {
            Console.WriteLine(@"");
        }
        public void WriteInfoLine(string infoToWrite)
        {
            Console.WriteLine(infoToWrite);
        }
        //  TODO create overloaded method to allow string format e.g. some text{0}and more text {1}, string1, string2 etc. just like can use in system.Console.Write()
        //  e.g. for string.Format()
        public void WriteInfoLine(string format, params string[] args)
        {
            Console.WriteLine(format, args);
        }
        #endregion
    }
    public class TextFileInfoWriter : IInfoWriter
    {
        string _fullFilename;

        public TextFileInfoWriter(string fullFilename)
        {
            _fullFilename = fullFilename;

            //  Create a new (or overwrite an existing) file.
            using (StreamWriter writer = new StreamWriter(_fullFilename, false))
            {
            }
        }
        public TextFileInfoWriter(string fullFilename, string headerLineText) : this(fullFilename)
        {
            //  Create a header line.
            this.WriteInfoLine(headerLineText);
        }

        #region IInfoWriter Members

        public void WriteInfoLine()
        {
            //  Output a blank line.
            this.WriteInfoLine(""); 
        }

        public void WriteInfoLine(string infoToWrite)
        {
            //  Append to existing file.
            using (StreamWriter writer = new StreamWriter(_fullFilename, true))
            {
                writer.WriteLine(infoToWrite);
            }
        }
        public void WriteInfoLine(string format, params string[] args)
        {
            string fullString = string.Format(format, args);
            this.WriteInfoLine(fullString);
        }
        #endregion
    }

    public class Helper
    {
        protected IInfoWriter _writeInfo;

        public Helper()
        {
            //  Create dummy info writer for situations which don't need the output.
            _writeInfo = new DummyInfoWriter(); 
        }

        public Helper(IInfoWriter writeInfo)
        {
            _writeInfo = writeInfo;
        }

        /// <summary>
        /// Output a string list to file. First row in file contains a header. File is created or overwritten if it exists.
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <param name="headerLineText"></param>
        /// <param name="list"></param>
        public void WriteToFile(List<string> listToWrite, string fullFilename, string headerLineText)
        {
            //  Write to outputter the progress info.
            _writeInfo.WriteInfoLine(@"Write string list to newly created file: " + fullFilename);

            bool appendFileFlag = false;
            WriteToFile(listToWrite, fullFilename, headerLineText, appendFileFlag);
        }
        /// <summary>
        /// Output a string list to file. First row in file contains a header. File can be created or overwritten as required.
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <param name="headerLineText"></param>
        /// <param name="list"></param>
        /// <param name="appendFileFlag">true to append to existing file, false to overwrite.</param>
        public void WriteToFile(List<string> listToWrite, string fullFilename, string headerLineText, bool appendFileFlag)
        {
            //  Write to outputter the progress info.
            _writeInfo.WriteInfoLine(@"Append string list to existing file: " + fullFilename);

            //  TODO put error handling
            using (StreamWriter writer = new StreamWriter(fullFilename, appendFileFlag))
            {
                writer.WriteLine(headerLineText);
                foreach (string value in listToWrite)
                {
                    writer.WriteLine(value);
                }
            }
        }
    
    }
}
