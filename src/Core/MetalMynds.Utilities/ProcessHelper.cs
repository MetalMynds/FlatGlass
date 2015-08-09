using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics;

namespace MetalMynds.Utilities
{
    public class ProcessHelper
    {
        //[DllImport("kernel32.dll")]
        //public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("KERNEL32.DLL")]
        private static extern IntPtr OpenProcess(eDesiredAccess dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr hObject);

        [DllImport("NTDLL.DLL")]
        private static extern int NtQueryInformationProcess(IntPtr hProcess, PROCESSINFOCLASS pic, ref PROCESS_BASIC_INFORMATION pbi, int cb, ref int pSize);

        [DllImport("NTDLL.DLL")]
        private static extern int NtQueryInformationProcess(IntPtr hProcess, PROCESSINFOCLASS pic, ref UNICODE_STRING str, int cb, ref int pSize);

        [DllImport("NTDLL.DLL")]
        private static extern int NtQueryInformationProcess(IntPtr hProcess, PROCESSINFOCLASS pic, IntPtr str, int cb, ref int pSize);

        [DllImport("ntdll.dll")]
        private static extern int NtQueryObject(IntPtr ObjectHandle, int
            ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength,
            ref int returnLength);

        [Flags]
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        private enum PROCESSINFOCLASS : int
        {

            ProcessBasicInformation = 0,
            ProcessQuotaLimits,
            ProcessIoCounters,
            ProcessVmCounters,
            ProcessTimes,
            ProcessBasePriority,
            ProcessRaisePriority,
            ProcessDebugPort,
            ProcessExceptionPort,
            ProcessAccessToken,
            ProcessLdtInformation,
            ProcessLdtSize,
            ProcessDefaultHardErrorMode,
            ProcessIoPortHandlers, // Note: this is kernel mode only
            ProcessPooledUsageAndLimits,
            ProcessWorkingSetWatch,
            ProcessUserModeIOPL,
            ProcessEnableAlignmentFaultFixup,
            ProcessPriorityClass,
            ProcessWx86Information,
            ProcessHandleCount,
            ProcessAffinityMask,
            ProcessPriorityBoost,
            MaxProcessInfoClass,
            ProcessWow64Information = 26,
            ProcessImageFileName = 27
        };

        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        //struct UNICODE_STRING
        //{
        //    public ushort Length;
        //    public ushort MaximumLength;
        //    public string Buffer;
        //}

        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            private IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            public void Dispose()
            {
                try
                {
                    Marshal.FreeHGlobal(buffer);
                }
                catch
                {
                }
                buffer = IntPtr.Zero;
            }

            public override string ToString()
            {
                String result = Marshal.PtrToStringUni(buffer);
                return result;
            }
        }

        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        //struct UNICODE_STRING
        //{
        //    public ushort Length;
        //    public ushort MaximumLength;
        //    public string Buffer;
        //}

        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        //public struct UNICODE_STRING
        //{
        //    public ushort Length;
        //    public ushort MaximumLength;
        //    public IntPtr Buffer;
        //}

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESS_BASIC_INFORMATION
        {
            public int ExitStatus;
            public int PebBaseAddress;
            public int AffinityMask;
            public int BasePriority;
            public int UniqueProcessId;
            public int InheritedFromUniqueProcessId;

            public int Size
            {
                get { return (6 * 4); }
            }
        };

        enum eDesiredAccess : int
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,
            STANDARD_RIGHTS_ALL = 0x001F0000,

            PROCESS_TERMINATE = 0x0001,
            PROCESS_CREATE_THREAD = 0x0002,
            PROCESS_SET_SESSIONID = 0x0004,
            PROCESS_VM_OPERATION = 0x0008,
            PROCESS_VM_READ = 0x0010,
            PROCESS_VM_WRITE = 0x0020,
            PROCESS_DUP_HANDLE = 0x0040,
            PROCESS_CREATE_PROCESS = 0x0080,
            PROCESS_SET_QUOTA = 0x0100,
            PROCESS_SET_INFORMATION = 0x0200,
            PROCESS_QUERY_INFORMATION = 0x0400,
            PROCESS_ALL_ACCESS = SYNCHRONIZE | 0xFFF
        }

        private static bool Is64Bits()
        {
            return Marshal.SizeOf(typeof(IntPtr)) == 8 ? true : false;
        }

        protected ManagementObjectSearcher BaseMOS = new ManagementObjectSearcher();

        public ProcessHelper()
        {
            BaseMOS = new ManagementObjectSearcher();
        }

        public String GetProcessCommand(Process Process)
        {
            lock (BaseMOS)
            {

                BaseMOS.Query = new ObjectQuery("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + Process.Id);

                foreach (var obj in BaseMOS.Get())
                {
                    object data = obj.Properties["CommandLine"].Value;
                    if (data != null)
                    {
                        return data.ToString();
                    }
                }

            }

            return string.Empty;

        }

        public static String GetProcessCommand(IntPtr Handle)
        {
            var objStr = new UNICODE_STRING(new string(' ', 512));

            //var objStr = new UNICODE_STRING();
            //IntPtr ipStr = IntPtr.Zero;
            String result = String.Empty;

            int pSize = 0;

            try
            {

                //IntPtr ipTemp = IntPtr.Zero;

                //String s = String.Empty;
                //s = s.PadLeft(512, ' ');

                //objStr.Length = (ushort)(s.Length * 2);
                //objStr.MaximumLength = (ushort)(objStr.Length + 2);
                //objStr.Buffer = Marshal.StringToHGlobalUni(s);

                //ipStr = Marshal.AllocHGlobal(Marshal.SizeOf(objStr));
                if (-1 != NtQueryInformationProcess(Handle, PROCESSINFOCLASS.ProcessImageFileName, ref objStr, Marshal.SizeOf(objStr), ref pSize))
                {
                    //objStr = (UNICODE_STRING)Marshal.PtrToStructure(ipStr, objStr.GetType());

                    //if (Is64Bits())
                    //{
                    //    ipTemp = new IntPtr(Convert.ToInt64(objStr.Buffer.ToString(), 10) >> 32);
                    //}
                    //else
                    //{
                    //    ipTemp = objStr.Buffer;
                    //}

                    //result = Marshal.PtrToStringUni(ipTemp, objStr.Length >> 1);
                    result = objStr.ToString();
                    objStr.Dispose();
                    //str.Dispose();

                    return result;
                    //return String.Empty;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                //Marshal.FreeHGlobal(ipStr);
            }
        }

        public static int GetParentProcessId(int PID)
        {
            int ParentID = 0;

            try
            {
                IntPtr hProcess =
                OpenProcess(eDesiredAccess.PROCESS_QUERY_INFORMATION, false, PID);
                if (hProcess != IntPtr.Zero)
                {
                    PROCESS_BASIC_INFORMATION pbi = new
                    PROCESS_BASIC_INFORMATION();
                    int pSize = 0;
                    if (-1 != NtQueryInformationProcess(hProcess,
                    PROCESSINFOCLASS.ProcessBasicInformation, ref pbi, pbi.Size, ref pSize))
                    {
                        ParentID = pbi.InheritedFromUniqueProcessId;
                    }

                    CloseHandle(hProcess);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(String.Format("ProcessHelper:\nError in GetParentProcessId(): {0}", e.Message));
            }

            return (ParentID);
        }
    }
}
