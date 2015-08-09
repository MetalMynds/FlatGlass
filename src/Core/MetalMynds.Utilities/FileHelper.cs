using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MetalMynds.Utilities
{
    public class FileHelper
    {
        public static String GetProductVersion(String Filename)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Filename);

            return String.Format("{0}.{1}.{2}.{3}", versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart);
        }

        public static Boolean TryGetTextFile(String Location, out String TextFile, out String Error)
        {
            if (Location.StartsWith("http://"))
            {
                Location = Location.Replace(@"\", @"/");
            }

            Uri uri = new Uri(Location);

            if (uri.IsUnc || uri.IsFile)
            {
                TextFile = File.ReadAllText(Location);
                Error = String.Empty;
                return true;
            }
            else
            {
                MemoryStream file;

                if (WGETHelper.TryGet(Location, out file, out Error))
                {
                    TextFile = Encoding.UTF8.GetString(file.ToArray());
                    Error = String.Empty;
                    return true;
                }
                else
                {
                    TextFile = String.Empty;
                    return false;
                }
            }
        }

        public static void DirectoryCopy(string Source, string Destination, bool Recursive = true, bool Overwrite = false)
        {
            DirectoryInfo dir = new DirectoryInfo(Source);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + Source);
            }

            if (!Directory.Exists(Destination))
            {
                Directory.CreateDirectory(Destination);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(Destination, file.Name);
                file.CopyTo(temppath, Overwrite);
            }

            if (Recursive)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(Destination, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, Recursive, Overwrite);
                }
            }
        }

        public static Boolean TryCopy(String SourcePath, String TargetPath, String WildCard, Boolean Recursive, Boolean Slient, out String Error)
        {
            Boolean notCopied = true;
            Boolean copyFailed = false;
            Boolean copyAbortedIgnored = false;
            Dictionary<String, String> failedCopies = new Dictionary<string, string>();

            if (!Directory.Exists(TargetPath))
            {
                Directory.CreateDirectory(TargetPath);
            }

            while (notCopied && !copyFailed && !copyAbortedIgnored)
            {
                Listing copyListing = new FileListing();

                copyListing.Search(SourcePath, WildCard, Recursive);

                int copyCount = 1;

                foreach (String sourceFilename in copyListing.Results)
                {
                    String targetFilename = String.Format("{0}\\{1}", TargetPath, Path.GetFileName(sourceFilename));

                    notCopied = true;

                    while (!copyAbortedIgnored && notCopied)
                    {
                        try
                        {
                            File.Copy(sourceFilename, targetFilename);

                            notCopied = false;
                        }
                        catch (Exception ex)
                        {
                            if (Slient)
                            {
                                failedCopies.Add(sourceFilename, ex.ToString());
                                copyFailed = true;
                                break;
                            }
                            else
                            {
                                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(String.Format("Copy of File [{0}/{1}] Failed!\nSource Filename: [{2}]\nTarget Filename: [{3}]\nError: {4}\n\nDo you Wish to Abort Copy, Retry or Ignore?", copyCount, copyListing.Results, SourcePath, TargetPath, ex.Message), "Copy File Error", System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore);

                                switch (result)
                                {
                                    case System.Windows.Forms.DialogResult.Ignore:
                                        failedCopies.Add(SourcePath, String.Format("{0}\nUser Action: Ignored", ex.ToString()));
                                        break;
                                    case System.Windows.Forms.DialogResult.Abort:
                                        copyAbortedIgnored = true;
                                        break;
                                    case System.Windows.Forms.DialogResult.Retry:
                                        copyAbortedIgnored = false;
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            bool sucess = !copyFailed && !copyAbortedIgnored && notCopied == false;

            if (sucess)
            {
                Error = String.Empty;
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                builder.AppendLine(String.Format("Copy Failed!\n  {0} Failed Copy(s)", failedCopies.Count));

                foreach (String failedFilename in failedCopies.Keys)
                {
                    builder.AppendLine(String.Format("  Filename: [{0}]\n  Error: [{1}]", failedFilename, failedCopies[failedFilename]));
                }

                Error = builder.ToString();
            }

            return sucess;
        }

        //public static String[] ReadAllLines(String Filename)
        //{
        //    List<String> lines = new List<string>();

        //    FileStream stream = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //    StreamReader reader = new StreamReader(stream);

        //    try
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            lines.Add(reader.ReadLine());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }

        //        if (stream != null)
        //        {
        //            stream.Close();
        //        }
        //    }

        //    return lines.ToArray<String>();

        //}

        public static Boolean IsIdentical(String SourceFilename, String DestinationFilename)
        {
            Byte[] sourceFile = File.ReadAllBytes(SourceFilename);
            Byte[] destinationFile = File.ReadAllBytes(DestinationFilename);

            return CompareHelper.Compare(sourceFile, destinationFile);
        }


        [DllImport("kernel32.dll")]
        static extern uint GetFileSize(IntPtr hFile, IntPtr lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            IntPtr lpFileMappingAttributes,
            FileMapProtection flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            [MarshalAs(UnmanagedType.LPTStr)]string lpName);

        [Flags]
        enum FileMapProtection : uint
        {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            FileMapAccess dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [Flags]
        enum FileMapAccess : uint
        {
            FileMapCopy = 0x0001,
            FileMapWrite = 0x0002,
            FileMapRead = 0x0004,
            FileMapAllAccess = 0x001f,
            fileMapExecute = 0x0020,
        }

        [DllImport("psapi.dll", SetLastError = true)]
        static extern uint GetMappedFileName(IntPtr m_hProcess, IntPtr lpv, StringBuilder
                lpFilename, uint nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public static string GetFileNameFromHandle(IntPtr FileHandle)
        {
            string fileName = String.Empty;
            IntPtr fileMap = IntPtr.Zero, fileSizeHi = IntPtr.Zero;
            UInt32 fileSizeLo = 0;

            fileSizeLo = GetFileSize(FileHandle, fileSizeHi);

            if (fileSizeLo == 0)
            {
                // cannot map an 0 byte file
                return null; ;
            }

            fileMap = CreateFileMapping(FileHandle, IntPtr.Zero, FileMapProtection.PageReadonly, 0, 1, null);

            if (fileMap != IntPtr.Zero)
            {
                IntPtr pMem = MapViewOfFile(fileMap, FileMapAccess.FileMapRead, 0, 0, 1);
                if (pMem != IntPtr.Zero)
                {
                    StringBuilder fn = new StringBuilder(250);
                    GetMappedFileName(System.Diagnostics.Process.GetCurrentProcess().Handle, pMem, fn, 250);
                    if (fn.Length > 0)
                    {
                        UnmapViewOfFile(pMem);
                        CloseHandle(FileHandle);
                        return fn.ToString();
                    }
                    else
                    {
                        UnmapViewOfFile(pMem);
                        CloseHandle(FileHandle);
                        return null;
                    }
                }
            }

            return null;
        }
    }



}