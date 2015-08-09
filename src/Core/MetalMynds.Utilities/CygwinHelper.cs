using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MetalMynds.Utilities
{
    public class CygwinHelper
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public delegate int MyFunction();

        static void Main(string[] args)
        {
            //load cygwin dll
            IntPtr pcygwin = LoadLibrary("cygwin1.dll");
            IntPtr pcyginit = GetProcAddress(pcygwin, "cygwin_dll_init");
            Action init = (Action)Marshal.GetDelegateForFunctionPointer(pcyginit, typeof(Action));
            init();

            IntPtr phello = LoadLibrary("hello.dll");
            IntPtr pfn = GetProcAddress(phello, "helloworld");
            MyFunction helloworld = (MyFunction)Marshal.GetDelegateForFunctionPointer(pfn, typeof(MyFunction));

            Console.WriteLine(helloworld());
            Console.ReadKey();
        }

    }
}
