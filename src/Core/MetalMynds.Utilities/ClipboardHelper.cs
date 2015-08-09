using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace MetalMynds.Utilities
{
    public class ClipboardHelper
    {
        public enum ClipboardFormats
        {
            Unknown,
            Text
//            Xml
        }
        
        protected static Object BaseData;

        public static void SaveClipboard()
        {
            BaseData = Clipboard.GetDataObject();
        }

        public static void RestoreClipboard()
        {
            Clipboard.SetDataObject(BaseData);
        }

        public static bool Copy(String Text)
        {
            Boolean finished = false;
            int timeout = 1;

            while (!finished && timeout <= 20)
            {
                try
                {
                    System.Windows.Forms.Clipboard.SetText(Text);
                    finished = true;
                }
                catch
                {
                    System.Threading.Thread.Sleep(250);
                    timeout += 1;
                }

            }

            return finished;
            
        }

        public static String Get()
        {
            return Clipboard.GetText();
        }

        public static Object Get(String Format)
        {
            return Clipboard.GetData(Format);
        }

        public static Object Get(DataFormats.Format Format)
        {
            return Clipboard.GetData(Format.Name);
        }

        public static bool Copy(Object Data)
        {
            return Copy(Data, false);
        }

        public static String GenerateFormatName(Object Source)
        {
            return Source.GetType().FullName;
        }

        public static String GenerateFormatName(Type Source)
        {
            return Source.FullName;
        }

        public static bool Copy(Object Data, Boolean AvailableAfterExit)
        {
            try
            {
                DataFormats.Format dataFormat = CreateCustomFormat(GenerateFormatName(Data));

                DataObject dataObject = new DataObject(dataFormat.Name, Data);

                Clipboard.SetDataObject(dataObject, AvailableAfterExit, 30, 250);

                return true;

            }
            catch
            {
                return false;
            }
            
        }

        public static bool Copy(Object Data, String CustomFormat, Boolean AvailableAfterExit)
        {
            try
            {
                DataFormats.Format dataFormat = CreateCustomFormat(CustomFormat);

                DataObject dataObject = new DataObject(dataFormat.Name, Data);

                Clipboard.SetDataObject(dataObject, AvailableAfterExit, 30, 250);

                return true;

            }
            catch
            {
                return false;
            }

        }

        public static DataFormats.Format CreateCustomFormat(String Name)
        {
            DataFormats.Format newFormat;

            try
            {

                newFormat = DataFormats.GetFormat(Name);

                return newFormat;

            }
            catch (Exception ex)
            {
                throw new System.Exception(String.Format("ClipboardHelper:\nError Unable To Create Custom Format Named [{0}]\nInner Exception: {1}", Name, ex), ex);
            }

        }

        public static bool ClipboardContains(DataFormats.Format Format)
        {
            return Clipboard.ContainsData(Format.Name);
        }

        public static bool ClipboardContains(Object Target)
        {

            DataFormats.Format targetFormat = CreateCustomFormat(GenerateFormatName(Target));

            return Clipboard.ContainsData(targetFormat.Name);

        }

        public static bool ClipboardContains(Type Target)
        {

            DataFormats.Format targetFormat = CreateCustomFormat(GenerateFormatName(Target));

            return Clipboard.ContainsData(targetFormat.Name);

        }

    }
}
